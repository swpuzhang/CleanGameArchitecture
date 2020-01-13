using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Account.Application.Services;
using Account.ViewModels;
using Commons.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Account.Controllers
{
    /// <summary>
    /// 账号相关操作
    /// </summary>
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _service;
        public AccountController(IAccountService service)
        {
            _service = service;
        }

        [HttpGet]
        public  WrappedResponse<AccountResponseVm> Test()
        {
            return new WrappedResponse<AccountResponseVm>();
        }

        /// <summary>
        /// 登录接口
        /// </summary>
        /// <param name="accountInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<WrappedResponse<AccountResponseVm>> Login([FromBody] AccountInfoVm accountInfo)
        {
            return await _service.Login(accountInfo);
        }

        /// <summary>
        /// 获取自己的账号信息
        /// </summary>
        /// <returns></returns>
        
        [HttpGet]
        public async Task<WrappedResponse<AccountDetailVm>> GetSelfAccount([FromHeader]long id)
        {
            var response = await _service.GetSelfAccount(id);
            return response;

        }

        /// <summary>
        /// 获取其他玩家的信息
        /// </summary>
        /// <param name="id">忽略</param>
        /// <param name="otherId">其他玩家Id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<WrappedResponse<OtherAccountDetailVm>> GetOtherAccount([FromHeader]long id, Int64 otherId)
        {

            var response = await _service.GetOtherAccount(id, otherId);
            return response;
        }
    }
}