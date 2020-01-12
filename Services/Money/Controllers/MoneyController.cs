using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Money.Application.Services;
using Money.ViewModels;
using Commons.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Money.Controllers
{
    /// <summary>
    /// 账号相关操作
    /// </summary>
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class MoneyController : ControllerBase
    {
        [HttpGet]
        public  WrappedResponse<MoneyResponseVm> Test()
        {
            return new WrappedResponse<MoneyResponseVm>();
        }

 
    }
}