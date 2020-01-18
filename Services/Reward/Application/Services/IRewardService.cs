using Reward.ViewModels;
using Reward.Domain.Entitys;
using Commons.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Commons.Enums;
using Commons.DiIoc;

namespace Reward.Application.Services
{
    public interface IRewardService : IDependency
    {
        Task<WrappedResponse<RegisterRewardVm>> QueryRegisterReward(long id);
        Task<WrappedResponse<RewardInfoVm>> GetRegisterReward(long id);
        Task<WrappedResponse<LoginRewardVm>> QueryLoginReward(long id);
        Task<WrappedResponse<RewardInfoVm>> GetLoginReward(long id);
        Task<WrappedResponse<BankruptcyInfoVm>> QueryBankruptcy(long id);
        Task<WrappedResponse<RewardInfoVm>> GetBankruptcy(long id);
        Task InvitedFriendReward(long id, string platform, AccountType type);
        Task InvitedFriendRegistered(string platform, AccountType type);
    }
}