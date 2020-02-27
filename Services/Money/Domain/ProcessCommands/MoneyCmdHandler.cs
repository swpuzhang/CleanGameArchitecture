using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Money.ViewModels;
using Commons.Models;
using MediatR;
using Money.Infrastruct.Repository;
using Money.Domain.Entitys;
using Commons.Tools.Encryption;
using Commons.Buses;
using Money.Domain.ProcessEvents;
using Commons.Enums;
using CommonMessages.MqCmds;
using MassTransit;
using Commons.Buses.MqBus;
using Serilog;
using Commons.Tools.KeyGen;
using CommonMessages.MqEvents;

namespace Money.Domain.ProcessCommands
{
    public class MoneyCmdHandler : IRequestHandler<GetMoneyCommand, WrappedResponse<MoneyInfo>>,
        IRequestHandler<BuyInCommand, WrappedResponse<MoneyInfo>>,
        IRequestHandler<AddMoneyCommand, WrappedResponse<MoneyInfo>>
    {
        
        private readonly IMoneyRedisRepository _moneyRedisRep;
        private readonly IMoneyInfoRepository _moneyRep;
        private readonly IBusControl _busCtl;
        public MoneyCmdHandler(IMoneyRedisRepository MoneyRedisRep, IMoneyInfoRepository MoneyRep, IBusControl busCtl)
        {
            _moneyRedisRep = MoneyRedisRep;
            _moneyRep = MoneyRep;
            _busCtl = busCtl;
        }

        public async Task<WrappedResponse<MoneyInfo>> Handle(GetMoneyCommand request, CancellationToken cancellationToken)
        {

            var moneyInfo = await _moneyRedisRep.GetMoneyInfo(request.Id);
            if (moneyInfo == null)
            {
                using (var locker = _moneyRedisRep.Locker(KeyGenTool.GenUserKey(request.Id,
                    MoneyInfo.ClassName)))
                {
                    await locker.LockAsync();
                    moneyInfo = await _moneyRep.FindAndAdd(request.Id,
                        new MoneyInfo(request.Id, 0, 0, 0, 0, 0));
                    _ = _moneyRedisRep.SetMoneyInfo(moneyInfo);
                }

                if (moneyInfo == null)
                {
                    return new WrappedResponse<MoneyInfo>(ResponseStatus.GetMoneyError, null, null);
                }

            }
            WrappedResponse<MoneyInfo> response = new WrappedResponse<MoneyInfo>
                (ResponseStatus.Success, null, moneyInfo);
            Log.Debug($"GetMoneyCommand:{moneyInfo.CurCoins},{moneyInfo.Carry}");
            return response;
        }

        public async Task<WrappedResponse<MoneyInfo>> Handle(AddMoneyCommand request, CancellationToken cancellationToken)
        {
            using var locker = _moneyRedisRep.Locker(KeyGenTool.GenUserKey(request.Id,MoneyInfo.ClassName));
            await locker.LockAsync();
            Log.Debug($"AddMoneyCommand add begin:{request.AddCoins},{request.AddCarry} {request.AggregateId}");
            var moneyInfo = await _moneyRedisRep.GetMoneyInfo(request.Id);
            if (moneyInfo == null)
            {
                moneyInfo = await _moneyRep.FindAndAdd(request.Id,
                    new MoneyInfo(request.Id, 0, 0, 0, 0, 0));
            }

            if (request.AddCoins < 0 &&
                System.Math.Abs(request.AddCoins) >
                moneyInfo.CurCoins)
            {
                Log.Debug($"AddMoneyCommand add end:{request.AddCoins},{request.AddCarry} {request.AggregateId}--1");
                return new WrappedResponse<MoneyInfo>
                    (ResponseStatus.NoEnoughMoney, null, null);
            }

            if (request.AddCarry < 0 &&
                System.Math.Abs(request.AddCarry) > moneyInfo.Carry)
            {
                Log.Debug($"AddMoneyCommand add end:{request.AddCoins},{request.AddCarry} {request.AggregateId}--2");
                return new WrappedResponse<MoneyInfo>
                    (ResponseStatus.NoEnoughMoney, null, null);
            }
            moneyInfo.AddCoins(request.AddCoins);
            moneyInfo.AddCarry(request.AddCarry);
            moneyInfo.UpdateMaxCoins();
            long coinsChangedCount = request.AddCoins + request.AddCarry;

            var moneyevent = new MoneyChangedMqEvent
                  (moneyInfo.Id, moneyInfo.CurCoins,
                  moneyInfo.CurDiamonds, moneyInfo.MaxCoins,
                  moneyInfo.MaxDiamonds, coinsChangedCount, 0, request.Reason);
            _ = _busCtl.Publish(moneyevent);
           // _busCtl.PublishServerReqExt(moneyInfo.Id, moneyevent);
            await Task.WhenAll(_moneyRedisRep.SetMoneyInfo(moneyInfo),
                _moneyRep.ReplaceAsync(moneyInfo));
            Log.Debug($"AddMoneyCommand add end:{request.AddCoins},{request.AddCarry} {request.AggregateId} curCoins:{moneyInfo.CurCoins} curCarry:{moneyInfo.Carry}--3");
            
            return new WrappedResponse<MoneyInfo>(ResponseStatus.Success, null,
                new MoneyInfo(request.Id, moneyInfo.CurCoins, moneyInfo.CurDiamonds,
                moneyInfo.MaxCoins, moneyInfo.MaxDiamonds, moneyInfo.Carry));

        }

        public async Task<WrappedResponse<MoneyInfo>> Handle(BuyInCommand request, CancellationToken cancellationToken)
        {
            using var locker = _moneyRedisRep.Locker(KeyGenTool.GenUserKey(request.Id, MoneyInfo.ClassName));
            await locker.LockAsync();
            Log.Debug($"AddMoneyCommand add begin:{request.MinBuy},{request.MaxBuy}, {request.AggregateId}");
            var moneyInfo = await _moneyRedisRep.GetMoneyInfo(request.Id);
            if (moneyInfo == null)
            {
                moneyInfo = await _moneyRep.FindAndAdd(request.Id,
                    new MoneyInfo(request.Id, 0, 0, 0, 0, 0));
            }

            if (moneyInfo.CurCoins + moneyInfo.Carry < request.MinBuy)
            {
                return new WrappedResponse<MoneyInfo>
                    (ResponseStatus.NoEnoughMoney, null, null);
            }
            long realBuy = 0;
            if (moneyInfo.CurCoins + moneyInfo.Carry >= request.MaxBuy)
            {
                realBuy = request.MaxBuy;
            }
            else
            {
                realBuy = moneyInfo.CurCoins + moneyInfo.Carry;
            }
            long left = moneyInfo.Carry - realBuy;
            if (left < 0)
            {
                moneyInfo.AddCoins(left);
                moneyInfo.AddCarry(realBuy);
            }
            var moneyevent = new MoneyChangedMqEvent
                 (moneyInfo.Id, moneyInfo.CurCoins,
                 moneyInfo.CurDiamonds, moneyInfo.MaxCoins,
                 moneyInfo.MaxDiamonds, 0, 0, request.Reason);
            _ = _busCtl.Publish(moneyevent);
            await Task.WhenAll(_moneyRedisRep.SetMoneyInfo(moneyInfo), _moneyRep.ReplaceAsync(moneyInfo));
            return new WrappedResponse<MoneyInfo>(ResponseStatus.Success, null,
                new MoneyInfo(request.Id, moneyInfo.CurCoins, moneyInfo.CurDiamonds,
                moneyInfo.MaxCoins, moneyInfo.MaxDiamonds, moneyInfo.Carry));

        }
    }
}
