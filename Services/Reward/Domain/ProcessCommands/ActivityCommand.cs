using Commons.Buses.ProcessBus;
using System;
using System.Collections.Generic;
using System.Text;
using Commons.Models;
using Reward.Domain.Entitys;
using SangongMessages.MqCmds;
using MediatR;
using Reward.ViewModels;

namespace Reward.Domain.Commands
{
    public class GameActivityCommand : ProcessCommand<List<OneGameActivityInfoVm>>
    {
        public long Id  { get; private set; }
        public ActivityTypes Type { get; private set; }
        public GameActivityCommand(long id, ActivityTypes type)
        {
            Id = id;
            Type = type;
        }
    }

    public class GetGameActRewardCommand : ProcessCommand<WrappedResponse<RewardInfoVm>>
    {
        public GetGameActRewardCommand(long id, string actId, string subId)
        {
            Id = id;
            ActId = actId;
            SubId = subId;
        }

        public long Id { get; private set; }
        public string ActId { get; private set; }
        public string SubId { get; private set; }
    }

  
    public class AddActFromGamelogCommand : ProcessCommand<Unit>
    {
        public Dictionary<long, long> AllPlayers { get; private set; }
        public RoomTypes RoomType { get; private set; }
        public AddActFromGamelogCommand(Dictionary<long, long> allPlayers, RoomTypes roomType)
        {
            AllPlayers = allPlayers;
            RoomType = roomType;
        }
    }
}
