using Account.Domain.Entitys;
using Account.Infrastruct.Repository;
using Account.ViewModels;
using AutoMapper;
using Commons.Buses;
using Commons.Buses.MqBus;
using Commons.Enums;
using Commons.Models;
using MassTransit;
using MediatR;
using Messages.MqCmds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Account.Domain.ProcessCommands
{
    public class GetAccountCmdHandler :
         IRequestHandler<GetSelfAccountCommand, WrappedResponse<AccountDetailVm>>,

        IRequestHandler<GetOtherAccountCommand, WrappedResponse<OtherAccountDetailVm>>
    {
        private readonly IAllRedisRepository _redisRep;
        private readonly IMediatorHandler _bus;
        private readonly IRequestClient<GetMoneyMqCmd> _moneyClient;
        private readonly IMapper _mapper;

        public GetAccountCmdHandler(IAllRedisRepository accountRedisRep,
            IMediatorHandler bus, IRequestClient<GetMoneyMqCmd> moneyClient, IMapper mapper)
        {
            _redisRep = accountRedisRep;
            _bus = bus;
            _moneyClient = moneyClient;
            _mapper = mapper;
        }

        private async Task<AccountDetailVm> GetAccountDetail(long id)

        {
            var tAccount = _redisRep.GetAccountInfo(id);
            var tMoney = _moneyClient.GetResponseExt<GetMoneyMqCmd, WrappedResponse<MoneyMqResponse>>
                            (new GetMoneyMqCmd(id));

            var tLevel = _bus.SendCommand(new GetLevelInfoCommand(id));
            var tGame = _bus.SendCommand(new GetGameInfoCommand(id));
            await Task.WhenAll(tAccount, tMoney, tLevel, tGame);

            var accountInfo = tAccount.Result;
            var moneyInfores = tMoney.Result;
            var moneyInfo = new MoneyInfoVm(moneyInfores.Message.Body.CurCoins + moneyInfores.Message.Body.Carry, 
                moneyInfores.Message.Body.CurDiamonds,
                moneyInfores.Message.Body.MaxCoins,
                moneyInfores.Message.Body.MaxDiamonds);
            var levelInfo = _mapper.Map<LevelInfoVm>(tLevel.Result.Body);
            var gameInfo = _mapper.Map <GameInfoVm >(tGame.Result.Body);

            if (accountInfo == null || moneyInfo == null || levelInfo == null || gameInfo == null)
            {
                return null;
            }
            return new AccountDetailVm(accountInfo.PlatformAccount,
                accountInfo.UserName, accountInfo.Sex, accountInfo.HeadUrl,
                accountInfo.Type, levelInfo, gameInfo, moneyInfo);
        }

        public async Task<WrappedResponse<AccountDetailVm>> Handle(GetSelfAccountCommand request, CancellationToken cancellationToken)
        {
            var accountInfo = await GetAccountDetail(request.Id);
            if (accountInfo == null)
            {
                return new WrappedResponse<AccountDetailVm>(ResponseStatus.AccountError);
            }

            return new WrappedResponse<AccountDetailVm>(ResponseStatus.Success,
                null, accountInfo);
        }

        public async Task<WrappedResponse<OtherAccountDetailVm>> Handle(GetOtherAccountCommand request, CancellationToken cancellationToken)
        {
            var accountInfo = await GetAccountDetail(request.OtherId);
            if (accountInfo == null)
            {
                return new WrappedResponse<OtherAccountDetailVm>(ResponseStatus.AccountError, null);
            }
            var otherinfo = _mapper.Map<OtherAccountDetailVm>(accountInfo);

            //var response = await _friednClient.GetResponseExt<GetFriendInfoMqCmd, WrappedResponse<GetFriendInfoMqResponse>>(
            //    new GetFriendInfoMqCommand(request.Id, request.OtherId));
            //if (response.Message.Body != null)
            //{
             //   otherinfo.FriendType = response.Message.Body.FriendType;
            //}
            return new WrappedResponse<OtherAccountDetailVm>(ResponseStatus.Success, null, otherinfo);
        }
    }
}
