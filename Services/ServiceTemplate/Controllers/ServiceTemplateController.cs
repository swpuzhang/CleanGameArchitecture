using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceTemplate.Application.Services;
using ServiceTemplate.ViewModels;
using Commons.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ServiceTemplate.Controllers
{
    /// <summary>
    /// 账号相关操作
    /// </summary>
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class ServiceTemplateController : ControllerBase
    {
        private readonly IServiceTemplateService _service;
        public ServiceTemplateController(IServiceTemplateService service)
        {
            _service = service;
        }

        [HttpGet]
        public  WrappedResponse<ServiceTemplateResponseVm> Test()
        {
            return new WrappedResponse<ServiceTemplateResponseVm>();
        }

        /// <summary>
        /// handle接口
        /// </summary>
        /// <param name="ServiceTemplateInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<WrappedResponse<ServiceTemplateResponseVm>> Login([FromBody] ServiceTemplateInfoVm ServiceTemplateInfo)
        {
            return await _service.Handle(ServiceTemplateInfo);
        }
    }
}