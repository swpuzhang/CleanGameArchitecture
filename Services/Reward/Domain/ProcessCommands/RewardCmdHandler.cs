using Reward.Domain.Commands;
using Reward.Domain.Entitys;
using Reward.Infrastruct.Repository;
using Commons.Buses;
using Commons.Enums;
using Commons.Models;
using Commons.Extenssions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using CommonMessages.MqEvents;
using Reward.ViewModels;
using CommonMessages.MqCmds;
using Commons.Buses.MqBus;
using Commons.Tools.Time;
using Commons.Tools.KeyGen;

namespace Reward.Domain.CommandHandlers
{
    public class RewardCommandHandler :
        IRequestHandler<QueryRegisterRewardCommand, WrappedResponse<RegisterRewardVm>>,
        IRequestHandler<GetRegisterRewardCommand, WrappedResponse<RewardInfoVm>>
    {
        //private readonly readonly IRequestClient<DoSomething> _requestClient;
    
        protected readonly IMediatorHandler _bus;
        private readonly IRegisterRewardRepository _registerRepository;
        private readonly IRewardRedisRepository _redis;
        private readonly IRequestClient<GetAccountBaseInfoMqCmd> _accountClient;
        private readonly IBusControl _mqBus;
        private readonly RegisterRewardConfig _regsterConfig;
        public RewardCommandHandler(IRegisterRewardRepository rep, IRewardRedisRepository redis,
            IMediatorHandler bus, IRequestClient<GetAccountBaseInfoMqCmd> accountClient,
            RegisterRewardConfig regsterConfig, IBusControl mqBus)
        {
            _registerRepository = rep;
            _redis = redis;
            _bus = bus;
            _accountClient = accountClient;
            _regsterConfig = regsterConfig;
            _mqBus = mqBus;
        }


        public async Task<WrappedResponse<RegisterRewardVm>> Handle(QueryRegisterRewardCommand request, CancellationToken cancellationToken)
        {
            //获取列表

            //获取该玩家的注册时间，从注册的第二天起才能领取注册奖励
            var accountResponse = await _accountClient.
                GetResponseExt<GetAccountBaseInfoMqCmd, WrappedResponse<GetAccountBaseInfoMqResponse>>
                (new GetAccountBaseInfoMqCmd(request.Id));
            var accountInfo = accountResponse.Message;
            if (accountInfo.ResponseStatus != ResponseStatus.Success)
            {
                return new WrappedResponse<RegisterRewardVm>(ResponseStatus.Error);
            }
            if ((accountInfo.Body.Flags & GetAccountBaseInfoMqResponse.SomeFlags.RegisterReward) ==
                GetAccountBaseInfoMqResponse.SomeFlags.RegisterReward)
            {
                return new WrappedResponse<RegisterRewardVm>(ResponseStatus.Success,
                    null, new RegisterRewardVm(RegisterRewardVm.RewardState.Over, 0, _regsterConfig.DayRewards));
            }
            if (_regsterConfig.DayRewards.Count == 0)
            {
                return new WrappedResponse<RegisterRewardVm>(ResponseStatus.Success,
                    null, new RegisterRewardVm(RegisterRewardVm.RewardState.None, 0, null));
            }
            DateTime registerDate = accountInfo.Body.RegisterDate.DateOfDayBegin();
            DateTime nowDate = DateTime.Now.DateOfDayBegin();
            if (registerDate == nowDate)
            {
                return new WrappedResponse<RegisterRewardVm>(ResponseStatus.Success,
                    null, new RegisterRewardVm(RegisterRewardVm.RewardState.NotBegin, 0, _regsterConfig.DayRewards));
            }

            int dayIndex = 0;
            var rewardInfo = await _redis.GetUserRegiserReward(request.Id);
            if (rewardInfo == null)
            {
                rewardInfo = await _registerRepository.GetByIdAsync(request.Id);

            }

            if (rewardInfo == null)
            {
                dayIndex = 0;
            }
            else
            {
                if (rewardInfo.DayIndex >= _regsterConfig.DayRewards.Count - 1)
                {
                    if (dayIndex >= _regsterConfig.DayRewards.Count - 1)
            {
                _ = _mqBus.Publish(new FinishedRegisterRewardMqEvent(request.Id));
            }
                    return new WrappedResponse<RegisterRewardVm>(ResponseStatus.Success,
                        null, new RegisterRewardVm(RegisterRewardVm.RewardState.Over, 0, _regsterConfig.DayRewards));
                }
                else if (rewardInfo.GetDate.DateOfDayBegin() == nowDate)
                {
                    return new WrappedResponse<RegisterRewardVm>(ResponseStatus.Success,
                        null, new RegisterRewardVm(RegisterRewardVm.RewardState.Getted, rewardInfo.DayIndex, _regsterConfig.DayRewards));
                }
                else
                {
                    dayIndex = rewardInfo.DayIndex + 1;
                }
            }
            return new WrappedResponse<RegisterRewardVm>(ResponseStatus.Success, null,
                new RegisterRewardVm(RegisterRewardVm.RewardState.Available, dayIndex, _regsterConfig.DayRewards));


        }

        public async Task<WrappedResponse<RewardInfoVm>> Handle(GetRegisterRewardCommand request, CancellationToken cancellationToken)
        {
            var accountResponse = await _accountClient.GetResponseExt<GetAccountBaseInfoMqCmd, WrappedResponse<GetAccountBaseInfoMqResponse>>
               (new GetAccountBaseInfoMqCmd(request.Id));
            var accountInfo = accountResponse.Message;
            if (accountInfo.ResponseStatus != ResponseStatus.Success)
            {
                return new WrappedResponse<RewardInfoVm>(ResponseStatus.Error);
            }
            if ((accountInfo.Body.Flags & GetAccountBaseInfoMqResponse.SomeFlags.RegisterReward) ==
                GetAccountBaseInfoMqResponse.SomeFlags.RegisterReward)
            {
                return new WrappedResponse<RewardInfoVm>(ResponseStatus.Error);
            }
            if (_regsterConfig.DayRewards.Count == 0)
            {
                return new WrappedResponse<RewardInfoVm>(ResponseStatus.Error);
            }
            DateTime registerDate = accountInfo.Body.RegisterDate.DateOfDayBegin();
            DateTime nowDate = DateTime.Now.DateOfDayBegin();
            if (registerDate == nowDate)
            {
                return new WrappedResponse<RewardInfoVm>(ResponseStatus.Error);
            }
            int dayIndex = 0;
            long rewardCoins = 0;
            using (var locker = _redis.Locker(KeyGenTool.GenUserKey(request.Id, nameof(RegisterRewardInfo))))
            {
                await locker.LockAsync();
                var rewardInfo = await _redis.GetUserRegiserReward(request.Id);
                if (rewardInfo == null)
                {
                    rewardInfo = await _registerRepository.GetByIdAsync(request.Id);

                }
                if (rewardInfo == null)
                {
                    rewardCoins = _regsterConfig.DayRewards[dayIndex];
                    dayIndex = 0;
                }
                else
                {
                    if (rewardInfo.DayIndex >= _regsterConfig.DayRewards.Count - 1)
                    {
                        return new WrappedResponse<RewardInfoVm>(ResponseStatus.Error);
                    }
                    else if (rewardInfo.GetDate.DateOfDayBegin() == nowDate)
                    {
                        return new WrappedResponse<RewardInfoVm>(ResponseStatus.Error);
                    }
                    else
                    {
                        dayIndex = rewardInfo.DayIndex + 1;
                        rewardCoins = _regsterConfig.DayRewards[dayIndex];
                    }
                }

                rewardInfo = new RegisterRewardInfo(request.Id, dayIndex, DateTime.Now);
                await Task.WhenAll(_redis.SetUserRegiserReward(rewardInfo), _registerRepository.ReplaceAndAddAsync(rewardInfo));
            }
            _ = _mqBus.Publish(new AddMoneyMqCmd(request.Id, rewardCoins, 0, AddReason.RegisterReward));
            if (dayIndex >= _regsterConfig.DayRewards.Count - 1)
            {
                _ = _mqBus.Publish(new FinishedRegisterRewardMqEvent(request.Id));
            }
            return new WrappedResponse<RewardInfoVm>(ResponseStatus.Success, null,
                    new RewardInfoVm(rewardCoins));
        }
    }
}
