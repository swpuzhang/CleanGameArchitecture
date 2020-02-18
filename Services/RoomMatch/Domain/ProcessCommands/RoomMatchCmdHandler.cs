using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RoomMatch.ViewModels;
using Commons.Models;
using MediatR;
using RoomMatch.Infrastruct.Repository;
using RoomMatch.Domain.Entitys;
using Commons.Tools.Encryption;
using Commons.Buses;
using RoomMatch.Domain.ProcessEvents;
using Commons.Enums;
using CommonMessages.MqCmds;
using MassTransit;
using Commons.Buses.MqBus;
using Serilog;
using RoomMatch.Manager;

namespace RoomMatch.Domain.ProcessCommands
{
    public class RoomMatchCmdHandler : 
        IRequestHandler<PlaynowCommand, WrappedResponse<RoomMatchResponseVm>>,
        IRequestHandler<BlindMatchCommand, WrappedResponse<RoomMatchResponseVm>>
    {

        protected readonly IMediatorHandler _bus;
        private readonly IRequestClient<GetMoneyMqCmd> _moneyClient;
 
        public RoomMatchCmdHandler(
            IMediatorHandler bus,
            IRequestClient<GetMoneyMqCmd> moneyClient)
        {
            _bus = bus;
            _moneyClient = moneyClient;
        }

        public async Task<WrappedResponse<RoomMatchResponseVm>> Handle(PlaynowCommand request, CancellationToken cancellationToken)
        {
            //获取玩家金币
            //根据金币判断玩家的场次
            var moneyResponse = await _moneyClient.GetResponseExt<GetMoneyMqCmd, WrappedResponse<MoneyMqResponse>>(new GetMoneyMqCmd(request.Id));
            if (moneyResponse.Message.ResponseStatus != ResponseStatus.Success)
            {
                return new WrappedResponse<RoomMatchResponseVm>(moneyResponse.Message.ResponseStatus, null);
            }
            long curCoins = moneyResponse.Message.Body.CurCoins;
            if (!MatchManager.GetBlindFromCoins(curCoins, out var blind))
            {
                return new WrappedResponse<RoomMatchResponseVm>
                    (ResponseStatus.NoEnoughMoney, null, null);
            }
            var response = await MatchManager.MatchRoom(request.Id, blind, "");
            //BodyResponse<SangongMatchingResponseInfo> response = new BodyResponse<SangongMatchingResponseInfo>(StatusCodeDefines.LoginError, null, null);
            return response;
        }

        public async Task<WrappedResponse<RoomMatchResponseVm>> Handle(BlindMatchCommand request, CancellationToken cancellationToken)
        {
            var moneyResponse = await _moneyClient.GetResponseExt<GetMoneyMqCmd, WrappedResponse<MoneyMqResponse>>(new GetMoneyMqCmd(request.Id));
            if (moneyResponse.Message.ResponseStatus != ResponseStatus.Success)
            {
                return new WrappedResponse<RoomMatchResponseVm>(moneyResponse.Message.ResponseStatus, null);
            }
            long curCoins = moneyResponse.Message.Body.CurCoins;
            if (!RoomManager.CoinsIsAvailable(curCoins, request.Blind))
            {
                return new WrappedResponse<RoomMatchResponseVm>(ResponseStatus.NoEnoughMoney, null);
            }
            var response = await MatchManager.MatchRoom(request.Id, request.Blind, "");
            return response;
        }
    }
}
