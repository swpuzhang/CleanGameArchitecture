using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Reward.Application.Services;
using Reward.ViewModels;
using Commons.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Reward.Controllers
{
    /// <summary>
    /// 账号相关操作
    /// </summary>
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class RewardController : ControllerBase
    {
        private readonly IRewardService _service;
        public RewardController(IRewardService service)
        {
            _service = service;
        }

        /// <summary>
        /// 查询注册奖励
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<WrappedResponse<RegisterRewardVm>> QueryRegisterReward([FromHeader]long id)
        {
            return await _service.QueryRegisterReward(id);
        }

        /// <summary>
        /// 领取注册奖励
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<WrappedResponse<RewardInfoVm>> GetRegisterReward([FromHeader]long id)
        {
            return await _service.GetRegisterReward(id);
        }

        /// <summary>
        /// 查询登录奖励
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<WrappedResponse<LoginRewardVm>> QueryLoginReward([FromHeader]long id)
        {
            return await _service.QueryLoginReward(id);
        }

        /// <summary>
        /// 获取登录奖励
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<WrappedResponse<RewardInfoVm>> GetLoginReward([FromHeader]long id)
        {
            return await _service.GetLoginReward(id);
        }

        /// <summary>
        /// 查询破产奖励
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<WrappedResponse<BankruptcyInfoVm>> QueryBankruptcy([FromHeader]long id)
        {
            return await _service.QueryBankruptcy(id);
        }

        /// <summary>
        /// 领取破产奖励
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<WrappedResponse<RewardInfoVm>> GetBankruptcy([FromHeader]long id)
        {
            return await _service.GetBankruptcy(id);
        }
    }
}