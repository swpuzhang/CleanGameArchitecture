using CommonMessages.MqEvents;
using MassTransit;
using Reward.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reward.WebApi.MqConsumers
{
    public class RewardMqConsumer :
        IConsumer<InviteFriendMqEvent>,
        IConsumer<RegistMqEvent>
       // ,IConsumer<GameLogMqCommand>
    {

        private readonly IRewardService _service;
        public RewardMqConsumer(IRewardService service)
        {
            _service = service;
        }

        public async Task Consume(ConsumeContext<InviteFriendMqEvent> context)
        {
            await _service.InvitedFriendReward(context.Message.Id, 
                context.Message.PlatformAccount, context.Message.Type);
        }

        public async Task Consume(ConsumeContext<RegistMqEvent> context)
        {
            await _service.InvitedFriendRegistered(context.Message.PlatformAccount, context.Message.Type);
        }

        /*public async Task Consume(ConsumeContext<GameLogMqCommand> context)
        {
            await _activityService.AddActFromGamelog(context.Message);
        }*/
    }
}
