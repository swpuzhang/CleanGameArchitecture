using MassTransit;
using CommonMessages.MqCmds;
using RoomMatch.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Money.MqConsumers
{
    /*public class RoomMatchConsumer :
         IConsumer<GetMoneyMqCmd>,
         IConsumer<AddMoneyMqCmd>
    {
        private IRoomMatchService _service;

        public RoomMatchConsumer(IRoomMatchService service)
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
