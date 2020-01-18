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
using CommonMessages.MqCmds;

namespace Reward.Domain.CommandHandlers
{
    public class LoginRewardCommandHandler :
        IRequestHandler<QueryLoginRewardCommand, WrappedResponse<LoginRewardVm>>,
        IRequestHandler<GetLoginRewardCommand, WrappedResponse<RewardInfoVm>>

    {
        protected readonly IMediatorHandler _bus;
        private readonly IRewardRedisRepository _redis;
        private readonly LoginRewardConfig _loginConfig;
        private readonly IBusControl _mqBus;

        public LoginRewardCommandHandler(IMediatorHandler bus, IRewardRedisRepository redis, 
            LoginRewardConfig loginConfig, IBusControl mqBus)
        {
            _bus = bus;
            _redis = redis;
            _loginConfig = loginConfig;
            _mqBus = mqBus;
        }

        public async Task<WrappedResponse<LoginRewardVm>> Handle(QueryLoginRewardCommand request, CancellationToken cancellationToken)
        {
            //查询本周redis之前的领奖记录
            DateTime tnow = DateTime.Now;
            var rewardInfo = await _redis.GetLoginReward(tnow, request.Id);
            return new WrappedResponse<LoginRewardVm>(ResponseStatus.Success, null, new LoginRewardVm(GenReward(tnow, rewardInfo)));
        }

        public List<LoginRewardVm.OneReward> GenReward(DateTime time, LoginRewardInfo reward)
        {
            List<LoginRewardVm.OneReward> daysReward = new List<LoginRewardVm.OneReward>();
            int dayOfWeek = (int)time.DayOfWeek;
            for (int i = 0; i < _loginConfig.DayRewards.Count; ++i)
            {
                LoginRewardVm.OneReward oneReward;
                if (dayOfWeek > i)
                {
                    if (reward == null || !reward.GettedDays.Contains(i))
                    {
                        oneReward = new LoginRewardVm.OneReward
                        {
                            DayIndex = i,
                            RewardCoins = _loginConfig.DayRewards[i],
                            State = LoginRewardVm.OneRewardState.NotGetted
                        };

                    }
                    else
                    {
                        oneReward = new LoginRewardVm.OneReward
                        {
                            DayIndex = i,
                            RewardCoins = _loginConfig.DayRewards[i],
                            State = LoginRewardVm.OneRewardState.Getted
                        };

                    }

                }

                else if (dayOfWeek < i)
                {
                    oneReward = new LoginRewardVm.OneReward
                    {
                        DayIndex = i,
                        RewardCoins = _loginConfig.DayRewards[i],
                        State = LoginRewardVm.OneRewardState.Waitting
                    };
                }
                else
                {
                    if (reward == null || !reward.GettedDays.Contains(i))
                    {
                        oneReward = new LoginRewardVm.OneReward
                        {
                            DayIndex = i,
                            RewardCoins = _loginConfig.DayRewards[i],
                            State = LoginRewardVm.OneRewardState.Available
                        };
                    }
                    else
                    {
                        oneReward = new LoginRewardVm.OneReward
                        {
                            DayIndex = i,
                            RewardCoins = _loginConfig.DayRewards[i],
                            State = LoginRewardVm.OneRewardState.Getted
                        };
                    }
                }
                daysReward.Add(oneReward);

            }
            return daysReward;
        }

        public async Task<WrappedResponse<RewardInfoVm>> Handle(GetLoginRewardCommand request, CancellationToken cancellationToken)
        {
            DateTime tnow = DateTime.Now;
            int dayOfWeek = (int)tnow.DayOfWeek;
            using var locker = _redis.Locker(KeyGenTool.GenUserWeekKey(tnow, request.Id, nameof(LoginRewardInfo)));

            var rewardInfo = await _redis.GetLoginReward(tnow, request.Id);
            long rewardCoins = 0;
            if (rewardInfo == null || !rewardInfo.GettedDays.Contains(dayOfWeek))
            {
                rewardCoins = _loginConfig.DayRewards[dayOfWeek];
                if (rewardInfo == null)
                {
                    rewardInfo = new LoginRewardInfo(request.Id, new List<int>());
                }
                rewardInfo.GettedDays.Add(dayOfWeek);
                await _redis.SetUserLoginReward(tnow, rewardInfo);
                _ = _mqBus.Publish(new AddMoneyMqCmd(request.Id, rewardCoins, 0, AddReason.LoginReward));
            }
            else
            {
                return new WrappedResponse<RewardInfoVm>(ResponseStatus.RewardGetted);
            }

            return new WrappedResponse<RewardInfoVm>(ResponseStatus.Success, null, new RewardInfoVm(rewardCoins));

        }
    }
}
