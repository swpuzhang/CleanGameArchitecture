using Commons.Buses;
using Commons.Models;
using Commons.Extenssions;
using Commons.Enums;
using MassTransit;
using MediatR;
using Reward.Domain.Commands;
using Reward.Domain.Entitys;
using Reward.Infrastruct.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Reward.ViewModels;
using Commons.Tools.KeyGen;
using Reward.Domain.Manager;
using CommonMessages.MqCmds;
using Commons.Buses.MqBus;

namespace Reward.Domain.CommandHandlers
{
    public class BankruptcyCommandHandler :
        IRequestHandler<QueryBankruptcyCommand, WrappedResponse<BankruptcyInfoVm>>,
        IRequestHandler<GetBankruptcyRewardCommand, WrappedResponse<RewardInfoVm>>

    {
        protected readonly IMediatorHandler _bus;
        private readonly IRewardRedisRepository _rewardRedis;
        private readonly IBusControl _mqBus;
        private readonly IRequestClient<GetMoneyMqCmd> _getMoneyClient;

        public BankruptcyCommandHandler(IMediatorHandler bus, IRewardRedisRepository redis,
             IBusControl mqBus, IRequestClient<GetMoneyMqCmd> getMoneyClient)
        {
            _bus = bus;
            _rewardRedis = redis;
            _mqBus = mqBus;
            _getMoneyClient = getMoneyClient;
        }

        public async Task<WrappedResponse<BankruptcyInfoVm>> Handle(QueryBankruptcyCommand request, CancellationToken cancellationToken)
        {
            //查询当天redis记录
            DateTime tnow = DateTime.Now;
            var bankruptcyInfo = await _rewardRedis.GetBankruptcyInfo(tnow, request.Id);
            int totalTimes = RewardManager.BankruptcyConf.BankruptcyRewards.Count;
            int curTimes;
            if (bankruptcyInfo == null)
            {
                curTimes = 0;
            }
            else
            {
                curTimes = bankruptcyInfo.CurTimes;
            }
            var getMoneyResponse = await _getMoneyClient.
               GetResponseExt<GetMoneyMqCmd, WrappedResponse<MoneyMqResponse>>
               (new GetMoneyMqCmd(request.Id));
            var moneyInfo = getMoneyResponse.Message;
            bool isAvailable = false;
            if (moneyInfo.ResponseStatus == ResponseStatus.Success)
            {
                if (moneyInfo.Body.CurCoins < RewardManager.BankruptcyConf.BankruptcyLimit)
                {
                    isAvailable = true;
                }
            }
            
            return new WrappedResponse<BankruptcyInfoVm>(ResponseStatus.Success, null,
                new BankruptcyInfoVm(BankruptcyInfoVm.BankruptcyRewardType.Day, totalTimes, curTimes,
                    RewardManager.BankruptcyConf.BankruptcyRewards, isAvailable));
        }

        public async Task<WrappedResponse<RewardInfoVm>> Handle(GetBankruptcyRewardCommand request, CancellationToken cancellationToken)
        {
            var getMoneyResponse = await _getMoneyClient.
              GetResponseExt<GetMoneyMqCmd, WrappedResponse<MoneyMqResponse>>
              (new GetMoneyMqCmd(request.Id));
            var moneyInfo = getMoneyResponse.Message;
            if (moneyInfo.ResponseStatus != ResponseStatus.Success)
            {
                return new WrappedResponse<RewardInfoVm>(ResponseStatus.RewardNotAvailable);
                
            }
            if (moneyInfo.Body.CurCoins > RewardManager.BankruptcyConf.BankruptcyLimit)
            {
                return new WrappedResponse<RewardInfoVm>(ResponseStatus.RewardNotAvailable);
            }

            DateTime tnow = DateTime.Now;
            using var locker = _rewardRedis.Locker(KeyGenTool.GenUserDayKey(tnow, request.Id, nameof(BankruptcyInfo)));

            await locker.LockAsync();
            var bankruptcyInfo = await _rewardRedis.GetBankruptcyInfo(tnow, request.Id);
            int totalTimes = RewardManager.BankruptcyConf.BankruptcyRewards.Count;
            if (bankruptcyInfo == null)
            {
                bankruptcyInfo = new BankruptcyInfo(0);
            }
            if (bankruptcyInfo.CurTimes >= 2)
            {
                return new WrappedResponse<RewardInfoVm>(ResponseStatus.RewardNotAvailable);
            }
            long rewardCoins = RewardManager.BankruptcyConf.BankruptcyRewards[bankruptcyInfo.CurTimes];
            ++bankruptcyInfo.CurTimes;
            await _rewardRedis.SetBankruptcyInfo(tnow, bankruptcyInfo);
            _ = _mqBus.Publish(new AddMoneyMqCmd(request.Id, rewardCoins, 0, AddReason.Bankruptcy));
            return new WrappedResponse<RewardInfoVm>(ResponseStatus.Success, null, new RewardInfoVm(rewardCoins));

        }
    }
}
