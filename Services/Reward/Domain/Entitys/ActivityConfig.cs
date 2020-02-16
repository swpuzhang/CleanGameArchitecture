using GameMessages.MqCmds;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reward.Domain.Entitys
{

    public enum ActivityTypes
    {
        PlayGame,
        WinGame
    }
    public class OneRoomActivityConfig
    {
        public OneRoomActivityConfig(string subId, int needCount,
            long rewardCoins, string title, RoomTypes roomType)
        {
            SubId = subId;
            NeedCount = needCount;
            RewardCoins = rewardCoins;
            Title = title;
            RoomType = roomType;
        }

        public string SubId { get; private set; }
        public long NeedCount { get; private set; }
        public long RewardCoins { get; private set; }
        public string Title { get; private set; }
        public RoomTypes RoomType { get; private set; }
    }

    public class GameActivityConfig
    {

        public GameActivityConfig(string activityId, string title,
            ActivityTypes activityType, 
            List<OneRoomActivityConfig> roomConfigs)
        {
            ActivityId = activityId;
            Title = title;
            ActivityType = activityType;
            RoomConfigs = roomConfigs;
        }

        /// <summary>
        /// 唯一活动ID
        /// </summary>
        public string ActivityId { get; private set; }
        public string Title { get; private set; }
        public ActivityTypes ActivityType { get; private set; }
        public List<OneRoomActivityConfig> RoomConfigs { get; private set; } 
    }

    /// <summary>
    /// 所有打牌活动的配置
    /// </summary>
    public class AllGameActivityConfig
    {
        public List<GameActivityConfig> AllGameConfigs { get; set; }
    }

}
