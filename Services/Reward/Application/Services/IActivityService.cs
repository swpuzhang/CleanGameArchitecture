using Reward.ViewModels;
using Reward.Domain.Entitys;
using Commons.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Commons.DiIoc;

namespace Reward.Application.Services
{
    public interface IActivityService : IDependency
    {
        Task<WrappedResponse<ActivityInfoVm>> QueryActivity(long id);
        Task<WrappedResponse<RewardInfoVm>> GetGameActReward(long id, string activityId, string subId);
        //Task AddActFromGamelog(GameLogMqCommand gamelog);
        

    }
}