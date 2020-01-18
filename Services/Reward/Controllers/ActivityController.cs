using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Commons.Models;
using Microsoft.AspNetCore.Mvc;
using Reward.Application.Services;
using Reward.Domain.Entitys;
using Reward.ViewModels;

namespace Reward.Controllers
{
    /// <summary>
    /// 活动相关接口
    /// </summary>
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class ActivityController : Controller
    {
        private readonly IActivityService _service;

        public ActivityController(IActivityService service)
        {
            _service = service;
        }

        /// <summary>
        /// 查询活动
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<WrappedResponse<ActivityInfoVm>> QueryActivity([FromHeader]long id)
        {
            return await _service.QueryActivity(id);
        }

       /// <summary>
       /// 获取打牌任务奖励
       /// </summary>
       /// <param name="id"></param>
       /// <param name="activityId"></param>
       /// <param name="subId"></param>
       /// <returns></returns>
        [HttpGet]
        public async Task<WrappedResponse<RewardInfoVm>> GetGameActReward([FromHeader]long id, string activityId, string subId)
        {
            return await _service.GetGameActReward(id, activityId, subId);
        }
    }
}