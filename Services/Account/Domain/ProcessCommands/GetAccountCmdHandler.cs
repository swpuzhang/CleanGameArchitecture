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
using CommonMessages.MqCmds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Account.Methods;

namespace Account.Domain.ProcessCommands
{
    public class GetAccountCmdHandler :
        IRequestHandler<GetSelfAccountCommand, WrappedResponse<AccountDetailVm>>,
        IRequestHandler<GetOtherAccountCommand, WrappedResponse<OtherAccountDetailVm>>,
        IRequestHandler<GetAccountBaseInfoCommand, WrappedResponse<GetAccountBaseInfoMqResponse>>,
        IRequestHandler<GetIdByPlatformCommand, WrappedResponse<GetIdByPlatformMqResponse>>



    {
        private readonly IAllRedisRepository _redisRep;
        private readonly IAccountInfoRepository _accountRep;
        private readonly IMediatorHandler _bus;
        private readonly IRequestClient<GetMoneyMqCmd> _moneyClient;
        private readonly IMapper _mapper;

        public GetAccountCmdHandler(IAllRedisRepository accountRedisRep,
            IAccountInfoRepository accountRep,
            IMediatorHandler bus, IRequestClient<GetMoneyMqCmd> moneyClient, IMapper mapper)
        {
            _redisRep = accountRedisRep;
            _bus = bus;
            _moneyClient = moneyClient;
            _mapper = mapper;
            _accountRep = accountRep;
        }

        private async Task<AccountDetailVm> GetAccountDetail(long id)

        {
            var accountInfo = await AccountRepositoryHelper.GetAccountInfo(id, _accountRep, _redisRep);
            if (accountInfo == null)
            {
                return null;
            }
            var tMoney = _moneyClient.GetResponseExt<GetMoneyMqCmd, WrappedResponse<MoneyMqResponse>>
                            (new GetMoneyMqCmd(id));

            var tLevel = _bus.SendCommand(new GetLevelInfoCommand(id));
            var tGame = _bus.SendCommand(new GetGameInfoCommand(id));
            await Task.WhenAll(tMoney, tLevel, tGame);

            var moneyResponse = tMoney.Result;
            var moneyInfo = new MoneyInfoVm(moneyResponse.Message.Body.CurCoins + moneyResponse.Message.Body.Carry,
                moneyResponse.Message.Body.CurDiamonds,
                moneyResponse.Message.Body.MaxCoins,
                moneyResponse.Message.Body.MaxDiamonds);
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

        public async Task<WrappedResponse<GetAccountBaseInfoMqResponse>> Handle(GetAccountBaseInfoCommand request, CancellationToken cancellationToken)
        {
            var accountInfo = await AccountRepositoryHelper.GetAccountInfo(request.Id, _accountRep, _redisRep);
            if (accountInfo == null)
            {
                return new WrappedResponse<GetAccountBaseInfoMqResponse>(ResponseStatus.AccountError, null);
            }
            return new WrappedResponse<GetAccountBaseInfoMqResponse>(ResponseStatus.Success, null, _mapper.Map<GetAccountBaseInfoMqResponse>(accountInfo));
        }

        public async Task<WrappedResponse<GetIdByPlatformMqResponse>> Handle(GetIdByPlatformCommand request, CancellationToken cancellationToken)
        {
            long? id = await AccountRepositoryHelper.GetIdByPlatForm(request.PlatformAccount, _accountRep, _redisRep);
            if (id != null)
            {
                return new WrappedResponse<GetIdByPlatformMqResponse>(ResponseStatus.Success, null, new GetIdByPlatformMqResponse(id.Value));
            }

            return new WrappedResponse<GetIdByPlatformMqResponse>(ResponseStatus.Error);
        }
    }
}
