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
using Money.Domain.ProcessCommands;

namespace Reward.Domain.CommandHandlers
{
    public class BankruptcyCommandHandler :
        IRequestHandler<QueryBankruptcyCommand, WrappedResponse<BankruptcyInfoVm>>,
        IRequestHandler<GetBankruptcyRewardCommand, WrappedResponse<RewardInfoVm>>

    {
        protected readonly IMediatorHandler _bus;
        private readonly IRewardRedisRepository _rewardRedis;
        private readonly BankruptcyConfig _bankruptcyConfig;
        private readonly IBusControl _mqBus;

        public BankruptcyCommandHandler(IMediatorHandler bus, IRewardRedisRepository redis,
            BankruptcyConfig bankruptcyConfig, IBusControl mqBus)
        {
            _bus = bus;
            _rewardRedis = redis;
            _bankruptcyConfig = bankruptcyConfig;
            _mqBus = mqBus;
        }

        public async Task<WrappedResponse<BankruptcyInfoVm>> Handle(QueryBankruptcyCommand request, CancellationToken cancellationToken)
        {
            //查询当天redis记录
            DateTime tnow = DateTime.Now;
            var bankruptcyInfo = await _rewardRedis.GetBankruptcyInfo(tnow, request.Id);
            int totalTimes = _bankruptcyConfig.BankruptcyRewards.Count;
            int curTimes;
            if (bankruptcyInfo == null)
            {
                curTimes = 0;
               
            }
            else
            {
                curTimes = bankruptcyInfo.CurTimes;
            }
            return new WrappedResponse<BankruptcyInfoVm>(ResponseStatus.Success, null,
                new BankruptcyInfoVm(BankruptcyInfoVm.BankruptcyRewardType.Day, totalTimes, curTimes,
                    _bankruptcyConfig.BankruptcyRewards));
        }

        public async Task<WrappedResponse<RewardInfoVm>> Handle(GetBankruptcyRewardCommand request, CancellationToken cancellationToken)
        {
            DateTime tnow = DateTime.Now;
            using var locker = _rewardRedis.Locker(KeyGenTool.GenUserDayKey(tnow, request.Id, nameof(BankruptcyInfo)));

            await locker.LockAsync();
            var bankruptcyInfo = await _rewardRedis.GetBankruptcyInfo(tnow, request.Id);
            int totalTimes = _bankruptcyConfig.BankruptcyRewards.Count;
            if (bankruptcyInfo == null)
            {
                bankruptcyInfo = new BankruptcyInfo(0);
            }
            if (bankruptcyInfo.CurTimes >= 2)
            {
                return new WrappedResponse<RewardInfoVm>(ResponseStatus.RewardNotAvailable);
            }
            long rewardCoins = _bankruptcyConfig.BankruptcyRewards[bankruptcyInfo.CurTimes];
            ++bankruptcyInfo.CurTimes;
            await _rewardRedis.SetBankruptcyInfo(tnow, bankruptcyInfo);
            _ = _mqBus.Publish(new AddMoneyCommand(request.Id, rewardCoins, 0, AddReason.Bankruptcy));
            return new WrappedResponse<RewardInfoVm>(ResponseStatus.Success, null, new RewardInfoVm(rewardCoins));

        }
    }
}
