using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Reward.ViewModels;
using Reward.Domain;
using Reward.Domain.Commands;
using Reward.Domain.Entitys;
using Reward.Infrastruct.Repository;
using AutoMapper;
using Commons.Buses;
using Commons.Models;
using Reward.Domain.Events;
using Commons.Enums;

namespace Reward.Application.Services
{
    public class RewardService : IRewardService
    {
        private readonly IMediatorHandler _bus;
        public RewardService(IMediatorHandler bus)
        {
            _bus = bus;
        }


        public async Task<WrappedResponse<RegisterRewardVm>> QueryRegisterReward(long id)
        {
            return await _bus.SendCommand(new QueryRegisterRewardCommand(id));
        }

        public async Task<WrappedResponse<RewardInfoVm>> GetRegisterReward(long id)
        {
            return await _bus.SendCommand(new GetRegisterRewardCommand(id));
        }

        public async Task<WrappedResponse<LoginRewardVm>> QueryLoginReward(long id)
        {
            return await _bus.SendCommand(new QueryLoginRewardCommand(id));
        }

        public async Task<WrappedResponse<RewardInfoVm>> GetLoginReward(long id)
        {
            return await _bus.SendCommand(new GetLoginRewardCommand(id));
        }

        public async Task<WrappedResponse<BankruptcyInfoVm>> QueryBankruptcy(long id)
        {
            return await _bus.SendCommand(new QueryBankruptcyCommand(id));
        }

        public async Task<WrappedResponse<RewardInfoVm>> GetBankruptcy(long id)
        {
            return await _bus.SendCommand(new GetBankruptcyRewardCommand(id));
        }

        public async Task InvitedFriendReward(long id, string platform, AccountType type)
        {
            await _bus.RaiseEvent(new InvitedFriendEvent(id, platform, type));
        }

        public async Task InvitedFriendRegistered(string platform, AccountType type)
        {
            await _bus.RaiseEvent(new InvitedFriendRegisterdEvent( platform, type));
        }
    }
}
