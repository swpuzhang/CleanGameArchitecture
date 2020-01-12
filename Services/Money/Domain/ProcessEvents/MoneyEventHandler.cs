using Money.Infrastruct.Repository;
using MediatR;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Money.Domain.ProcessEvents
{
    public class MoneyEventHandler : INotificationHandler<MoneyEvent>
    {
       
        public MoneyEventHandler()
        {
        }
        public Task Handle(MoneyEvent notification, CancellationToken cancellationToken)
        {
            
            return Task.CompletedTask;
        }
    }
}
