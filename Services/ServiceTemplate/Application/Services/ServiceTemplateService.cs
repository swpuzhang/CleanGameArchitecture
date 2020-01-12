using ServiceTemplate.ViewModels;
using AutoMapper;
using Commons.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Commons.Buses.ProcessBus;
using Commons.Buses;
using ServiceTemplate.Domain.ProcessCommands;
using ServiceTemplate.Domain.Entitys;

namespace ServiceTemplate.Application.Services
{
    public class ServiceTemplateService : IServiceTemplateService
    {
        private readonly IMapper _mapper;
        private readonly IMediatorHandler _bus;
        public ServiceTemplateService(IMapper mapper, IMediatorHandler bus)
        {
            _mapper = mapper;
            _bus = bus;
        }
        public Task<WrappedResponse<ServiceTemplateResponseVm>> Handle(ServiceTemplateInfoVm ServiceTemplateInfo)
        {
            return _bus.SendCommand(new ServiceTemplateCommand(_mapper.Map<ServiceTemplateInfo>(ServiceTemplateInfo)));
        
        }
    }
}
