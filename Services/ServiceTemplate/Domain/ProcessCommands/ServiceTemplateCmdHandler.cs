using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ServiceTemplate.ViewModels;
using Commons.Models;
using MediatR;
using ServiceTemplate.Infrastruct.Repository;
using ServiceTemplate.Domain.Entitys;
using Commons.Tools.Encryption;
using Commons.Buses;
using ServiceTemplate.Domain.ProcessEvents;
using Commons.Enums;
using CommonMessages.MqCmds;
using MassTransit;
using Commons.Buses.MqBus;
using Serilog;

namespace ServiceTemplate.Domain.ProcessCommands
{
    public class ServiceTemplateCmdHandler : IRequestHandler<ServiceTemplateCommand, WrappedResponse<ServiceTemplateResponseVm>>
    {
        
        /*private readonly IServiceTemplateRedisRepository _ServiceTemplateRedisRep;
        private readonly IServiceTemplateInfoRepository _ServiceTemplateRep;
        private readonly IMediatorHandler _bus;

        public ServiceTemplateCmdHandler(IServiceTemplateRedisRepository ServiceTemplateRedisRep, IServiceTemplateInfoRepository ServiceTemplateRep,
            IMediatorHandler bus)
        {
            _ServiceTemplateRedisRep = ServiceTemplateRedisRep;
            _ServiceTemplateRep = ServiceTemplateRep;
            _bus = bus;
        }*/

        public  Task<WrappedResponse<ServiceTemplateResponseVm>> Handle(ServiceTemplateCommand request, CancellationToken cancellationToken)
        {
            
            WrappedResponse<ServiceTemplateResponseVm> response = new WrappedResponse<ServiceTemplateResponseVm>(ResponseStatus.LoginError,
               null, null);

            return Task.FromResult(response);
        }
    }
}
