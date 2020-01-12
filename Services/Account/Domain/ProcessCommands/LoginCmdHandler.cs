using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Account.ViewModels;
using Commons.Models;
using MediatR;
using Account.Infrastruct.Repository;
using Account.Domain.Entitys;
using Commons.Tools.Encryption;
using Commons.Buses;
using Account.Domain.ProcessEvents;
using Commons.Enums;
using Messages.MqCmds;
using MassTransit;
using Commons.Buses.MqBus;

namespace Account.Domain.ProcessCommands
{
    public class LoginCmdHandler : IRequestHandler<LoginCommand, WrappedResponse<AccountResponse>>
    {
        
        private readonly IAllRedisRepository _redisRep;
        private readonly IAccountInfoRepository _accountRep;
        private readonly IUserIdGenRepository _genRepository;
        private readonly IMediatorHandler _bus;
        private readonly IRequestClient<GetMoneyMqCmd> _moneyClient;
        private readonly IRequestClient<AddMoneyMqCmd> _moneyAddClient;

        public LoginCmdHandler(IAllRedisRepository accountRedisRep, IAccountInfoRepository accountRep,
            IUserIdGenRepository genRepository, IMediatorHandler bus, IRequestClient<GetMoneyMqCmd> moneyClient, 
            IRequestClient<AddMoneyMqCmd> moneyAddClient)
        {
            _redisRep = accountRedisRep;
            _accountRep = accountRep;
            _genRepository = genRepository;
            _bus = bus;
            _moneyClient = moneyClient;
            _moneyAddClient = moneyAddClient;
        }

        public async Task<WrappedResponse<AccountResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var newAccountInfo = request.Info;
            //判断该平台ID是否已经注册, 先从redis查找
            var loginCheckInfo = await _redisRep.GetLoginCheckInfo(newAccountInfo.PlatformAccount);
            bool isRegister = false;
            AccountInfo accountInfo;
            if (loginCheckInfo != null)
            {
                //直接通过ID去查找这个玩家信息
                accountInfo = await _redisRep.GetAccountInfo(loginCheckInfo.Id);
                //为空从数据库读取
                if (accountInfo == null)
                {
                    accountInfo = await _accountRep.GetByIdAsync(loginCheckInfo.Id);
                }
            }
            else
            {
                //查找数据库中是否有这个账号
                accountInfo = await _accountRep.GetByPlatform(newAccountInfo.PlatformAccount);
                if (accountInfo == null)
                {
                    //注册新账号
                    isRegister = true;
                    long newUid = await _genRepository.GenNewId();
                    accountInfo = new AccountInfo(newUid, newAccountInfo.PlatformAccount,
                        newAccountInfo.UserName, newAccountInfo.Sex, newAccountInfo.HeadUrl,
                        newAccountInfo.Type, DateTime.Now);
                    await _accountRep.AddAsync(accountInfo);
                }
            }

            if (accountInfo != null)
            {
                newAccountInfo.Id = accountInfo.Id;
                string token = TokenTool.GenToken(accountInfo.Id);
                AccountResponse accounResponse;
                bool isNeedUpdate = false;
                if (!isRegister && accountInfo.IsNeedUpdate(newAccountInfo))
                {
                    isNeedUpdate = true;
                }
                if (isRegister)
                {
                    var mqResponse = await _moneyAddClient.GetResponseExt<AddMoneyMqCmd, WrappedResponse<MoneyMqResponse>>
                          (new AddMoneyMqCmd(accountInfo.Id, 1000, 0, AddReason.InitReward));
                    var moneyInfo = mqResponse.Message.Body;
                    accounResponse = new AccountResponse(newAccountInfo.Id,
                   newAccountInfo.PlatformAccount,
                   newAccountInfo.UserName,
                   newAccountInfo.Sex,
                   newAccountInfo.HeadUrl,
                   token, new MoneyInfo(moneyInfo.CurCoins + moneyInfo.Carry,
                    moneyInfo.CurDiamonds,
                    moneyInfo.MaxCoins,
                    moneyInfo.MaxDiamonds), "192.168.1.1", true,
                    newAccountInfo.Type);
                }
                else
                {
                    var mqResponse = await _moneyClient.GetResponseExt<GetMoneyMqCmd, WrappedResponse<MoneyMqResponse>>
                           (new GetMoneyMqCmd(accountInfo.Id));
                    var moneyInfo = mqResponse.Message.Body;
                    accounResponse = new AccountResponse(newAccountInfo.Id,
                  newAccountInfo.PlatformAccount,
                  newAccountInfo.UserName,
                  newAccountInfo.Sex,
                  newAccountInfo.HeadUrl,token,
                   new MoneyInfo(moneyInfo.CurCoins + moneyInfo.Carry,
                    moneyInfo.CurDiamonds,
                    moneyInfo.MaxCoins,
                    moneyInfo.MaxDiamonds), "192.168.1.1", false, newAccountInfo.Type);
                }

                _ = _bus.RaiseEvent<LoginEvent>(new LoginEvent(Guid.NewGuid(),
                    accounResponse, isRegister, isNeedUpdate, newAccountInfo));
                WrappedResponse<AccountResponse> retRresponse =
                    new WrappedResponse<AccountResponse>(ResponseStatus.Success, null, accounResponse);
                return retRresponse;
            }
            WrappedResponse<AccountResponse> response = new WrappedResponse<AccountResponse>(ResponseStatus.LoginError,
               null, null);

            return response;
        }
    }
}
