using ServiceTemplate.Domain.Entitys;
using ServiceTemplate.ViewModels;
using Commons.Buses.ProcessBus;
using Commons.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceTemplate.Domain.ProcessCommands
{
    public class ServiceTemplateCommand : ProcessCommand<WrappedResponse<ServiceTemplateResponseVm>>
    {
        public ServiceTemplateInfo Info { get; private set; }
        public ServiceTemplateCommand(ServiceTemplateInfo info)
        {
            Info = info;
        }
    }
}
