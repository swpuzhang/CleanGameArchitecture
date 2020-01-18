using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reward.ViewModels
{
    /// <summary>
    ///单个房间打牌活动
    /// </summary>
    public class OneRoomActivityInfoVm
    {
        /// <summary>
        /// 子任务ID
        /// </summary>
        public string SubId { get; set; }
        /// <summary>
        /// 当前计数
        /// </summary>
        public long CurCount { get; set; }
        /// <summary>
        /// 需要的总计数
        /// </summary>
        public long NeedCount { get; set; }
        /// <summary>
        /// 是否完成该任务
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 奖励金币数
        /// </summary>

        public long RewardCoins { get; set; }
        /// <summary>
        /// 活动标题
        /// </summary>
        public string Title { get; set; }
    }

    /// <summary>
    /// 一个打牌活动
    /// </summary>
    public class OneGameActivityInfoVm
    {
        public OneGameActivityInfoVm(string activityId, string title, List<OneRoomActivityInfoVm> roomInfos)
        {
            ActivityId = activityId;
            Title = title;
            RoomInfos = roomInfos;
        }

        /// <summary>
        /// 唯一活动ID
        /// </summary>
        public string ActivityId { get; set; }
        /// <summary>
        /// 活动标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 所有场次打牌详情
        /// </summary>
        public List<OneRoomActivityInfoVm> RoomInfos { get; set; }
    }

    /// <summary>
    /// 所有活动信息
    /// </summary>
    public class ActivityInfoVm
    {

        /// <summary>
        ///所有打牌活动
        /// </summary>
        public List<OneGameActivityInfoVm> AllGameActivitys { get; set; }
        /// <summary>
        ///所有赢牌活动
        /// </summary>
        public List<OneGameActivityInfoVm> AllWinActivitys { get; set; }
    }
}
