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
    public class ActivityService : IActivityService
    {
        private readonly IMediatorHandler _bus;
        public ActivityService(IMediatorHandler bus)
        {
            _bus = bus;
        }

        public Task<WrappedResponse<RewardInfoVm>> GetGameActReward(long id, string activityId, string subId)
        {

            return _bus.SendCommand(new GetGameActRewardCommand(id, activityId, subId));
        }

        public async Task<WrappedResponse<ActivityInfoVm>> QueryActivity(long id)
        {
            var playInfos = await _bus.SendCommand(new GameActivityCommand(id, ActivityTypes.PlayGame));
            var winInfos = await _bus.SendCommand(new GameActivityCommand(id, ActivityTypes.WinGame));
            return new WrappedResponse<ActivityInfoVm>(ResponseStatus.Success, null,
                new ActivityInfoVm
                {
                    AllGameActivitys = playInfos,
                    AllWinActivitys = winInfos
                });
        }

        /*public async Task AddActFromGamelog(GameLogMqCommand gamelog)
        {
            //玩家
            var allPlayers = gamelog.GetPlayers();
            await _bus.SendCommand(new AddActFromGamelogCommand(allPlayers, gamelog.GameLog.RoomType));
        }*/
    }
}
