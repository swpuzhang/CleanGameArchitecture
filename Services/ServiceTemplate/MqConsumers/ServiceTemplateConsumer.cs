using MassTransit;
using Messages.MqCmds;
using ServiceTemplate.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Money.MqConsumers
{
    /*public class ServiceTemplateConsumer :
         IConsumer<GetMoneyMqCmd>,
         IConsumer<AddMoneyMqCmd>
    {
        private IServiceTemplateService _service;

        public ServiceTemplateConsumer(IServiceTemplateService service)
        {
            _service = service;
        }

        public Task Consume(ConsumeContext<AddMoneyMqCmd> context)
        {
            throw new NotImplementedException();
        }

        public Task Consume(ConsumeContext<GetMoneyMqCmd> context)
        {
            throw new NotImplementedException();
        }
    }*/
}
