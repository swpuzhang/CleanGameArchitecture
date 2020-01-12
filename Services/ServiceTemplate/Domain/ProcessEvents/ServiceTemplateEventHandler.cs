using ServiceTemplate.Infrastruct.Repository;
using MediatR;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceTemplate.Domain.ProcessEvents
{
    public class ServiceTemplateEventHandler : INotificationHandler<ServiceTemplateEvent>
    {
        /*private readonly IServiceTemplateRedisRepository _ServiceTemplateRedisRep;
        private readonly IServiceTemplateInfoRepository _ServiceTemplateRep;
        public ServiceTemplateEventHandler(IServiceTemplateRedisRepository ServiceTemplateRedisRep, IServiceTemplateInfoRepository ServiceTemplateRep)
        {
            _ServiceTemplateRedisRep = ServiceTemplateRedisRep;
            _ServiceTemplateRep = ServiceTemplateRep;
        }*/
        public Task Handle(ServiceTemplateEvent notification, CancellationToken cancellationToken)
        {
            
            return Task.CompletedTask;
        }
    }
}
