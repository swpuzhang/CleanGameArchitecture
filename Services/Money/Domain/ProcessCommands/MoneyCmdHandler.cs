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
using Messages.MqCmds;
using MassTransit;
using Commons.Buses.MqBus;
using Serilog;
using Commons.Tools.KeyGen;

namespace Money.Domain.ProcessCommands
{
    public class MoneyCmdHandler : IRequestHandler<GetMoneyCommand, WrappedResponse<MoneyInfo>>
        , IRequestHandler<AddMoneyCommand, WrappedResponse<MoneyInfo>>
    {
        
        private readonly IMoneyRedisRepository _MoneyRedisRep;
        private readonly IMoneyInfoRepository _MoneyRep;

        public MoneyCmdHandler(IMoneyRedisRepository MoneyRedisRep, IMoneyInfoRepository MoneyRep)
        {
            _MoneyRedisRep = MoneyRedisRep;
            _MoneyRep = MoneyRep;
        }

        public async Task<WrappedResponse<MoneyInfo>> Handle(GetMoneyCommand request, CancellationToken cancellationToken)
        {

            var moneyInfo = await _MoneyRedisRep.GetMoneyInfo(request.Id);
            if (moneyInfo == null)
            {
                using (var locker = _MoneyRedisRep.Locker(KeyGenTool.GenUserKey(request.Id,
                    MoneyInfo.ClassName)))
                {
                    await locker.LockAsync();
                    moneyInfo = await _MoneyRep.FindAndAdd(request.Id,
                        new MoneyInfo(request.Id, 0, 0, 0, 0, 0));
                    _ = _MoneyRedisRep.SetMoneyInfo(moneyInfo);
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
            using var locker = _MoneyRedisRep.Locker(KeyGenTool.GenUserKey(request.Id,MoneyInfo.ClassName));
            await locker.LockAsync();
            Log.Debug($"AddMoneyCommand add begin:{request.AddCoins},{request.AddCarry} {request.AggregateId}");
            var moneyInfo = await _MoneyRedisRep.GetMoneyInfo(request.Id);
            if (moneyInfo == null)
            {
                moneyInfo = await _MoneyRep.FindAndAdd(request.Id,
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
            /*long coinsChangedCount = request.AddCoins + request.AddCarry;

            var moneyevent = new MoneyChangedMqEvent
                  (moneyInfo.Id, moneyInfo.CurCoins,
                  moneyInfo.CurDiamonds, moneyInfo.MaxCoins,
                  moneyInfo.MaxDiamonds, coinsChangedCount, 0);
            _busCtl.PublishExt(moneyevent);
            _busCtl.PublishServerReqExt(moneyInfo.Id, moneyevent);
            await Task.WhenAll(_redis.SetMoney(request.Id, moneyInfo),
                _moneyRepository.ReplaceAsync(moneyInfo));
            Log.Debug($"AddMoneyCommand add end:{request.AddCoins},{request.AddCarry} {request.AggregateId} curCoins:{moneyInfo.CurCoins} curCarry:{moneyInfo.Carry}--3");
            */
            return new WrappedResponse<MoneyInfo>(ResponseStatus.Success, null,
                new MoneyInfo(request.Id, moneyInfo.CurCoins, moneyInfo.CurDiamonds,
                moneyInfo.MaxCoins, moneyInfo.MaxDiamonds, moneyInfo.Carry));

        }
    }
}
