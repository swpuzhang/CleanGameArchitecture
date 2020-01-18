using MassTransit;
using CommonMessages.MqCmds;
using Money.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Money.MqConsumers
{
    public class MoneyMqConsumer :
         IConsumer<GetMoneyMqCmd>,
         IConsumer<AddMoneyMqCmd>
    {
        private readonly IMoneyService _service;

        public MoneyMqConsumer(IMoneyService service)
        {
            _service = service;
        }

        public async Task Consume(ConsumeContext<AddMoneyMqCmd> context)
        {
            var response = await _service.AddMoney(context.Message.Id, 
                context.Message.AddCoins, context.Message.AddCarry, context.Message.Reason);
            await context.RespondAsync(response);
        }

        public async Task Consume(ConsumeContext<GetMoneyMqCmd> context)
        {
            var response = await _service.GetMoney(context.Message.Id);
            await context.RespondAsync(response);
        }
    }
}
