using ServiceTemplate.ViewModels;
using Commons.DiIoc;
using Commons.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceTemplate.Application.Services
{
    public interface IServiceTemplateService : IDependency
    {
        public Task<WrappedResponse<ServiceTemplateResponseVm>> Handle(ServiceTemplateInfoVm ServiceTemplateInfo);
    }
}
