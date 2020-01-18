using AutoMapper;
using MassTransit;
using MediatR;
using Reward.Domain.Events;
using Reward.Domain.Entitys;
using Reward.Infrastruct.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommonMessages.MqCmds;
using System.Linq;
using Commons.Extenssions;

namespace Reward.Domain.EventHandlers
{
    public class RewardEventHandler : 
        INotificationHandler<InvitedFriendEvent>,
        INotificationHandler<InvitedFriendRegisterdEvent>
    {
        private readonly IRewardRedisRepository _redis;
        private readonly InviteRewardConfig _inviteConfig;
        private readonly IBusControl _mqBus;

        public RewardEventHandler(IRewardRedisRepository redis, InviteRewardConfig inviteConfig, IBusControl mqBus)
        {
            _redis = redis;
            _inviteConfig = inviteConfig;
            _mqBus = mqBus;
        }

        public Task Handle(InvitedFriendEvent notification, CancellationToken cancellationToken)
        {
            return _redis.SetInviteFriend(notification.Id, notification.PlatformAccount);
        }

        public async Task Handle(InvitedFriendRegisterdEvent notification, CancellationToken cancellationToken)
        {

            var allInviters = await _redis.GetInviter(notification.PlatformAccount);
            if (allInviters == null || allInviters.Count() == 0)
            {
                return;
            }
            long rewardCoins = _inviteConfig.InviteRewards;
            allInviters.ForEach(x => _mqBus.Publish(new AddMoneyMqCmd(x, rewardCoins, 0, Commons.Enums.AddReason.Invite)));
            await _redis.RemovInviteFriend(allInviters, notification.PlatformAccount);
        }
    }
}
