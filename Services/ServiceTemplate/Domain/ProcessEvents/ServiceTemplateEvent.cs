using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceTemplate.Domain.Entitys;
using ServiceTemplate.ViewModels;
using Commons.Buses.ProcessBus;

namespace ServiceTemplate.Domain.ProcessEvents
{
    public class ServiceTemplateEvent : ProcessEvent
    {
        public ServiceTemplateEvent()
        {
        }

        public ServiceTemplateEvent(Guid gid)
        {
            AggregateId = gid;;
        }

    }
}
