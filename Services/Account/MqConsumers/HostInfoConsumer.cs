using Account.Application.Services;
using CommonMessages.MqEvents;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account.MqConsumers
{
    public class HostInfoConsumer :
        IConsumer<HostInfoMqEvent>
    {
        private readonly IMqService _service;

        public HostInfoConsumer(IMqService service)
        {
            _service = service;
        }

        public Task Consume(ConsumeContext<HostInfoMqEvent> context)
        {
            _service.NotifyHostInfo(context.Message.Host, context.Message.UserCount);
            return Task.CompletedTask;
        }
    }
}
