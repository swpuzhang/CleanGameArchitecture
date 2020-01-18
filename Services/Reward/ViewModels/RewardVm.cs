using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Commons.Enums;

namespace Reward.ViewModels
{
    /// <summary>
    /// 接口字段
    /// </summary>
    public class RewardVm
    {
        public Int64 Id { get; set; }  
    }

    /// <summary>
    /// 注册奖励
    /// </summary>
    public class RegisterRewardVm
    {
        public enum RewardState
        {
            None = 0,
            NotBegin = 1,
            Available = 2,
            Getted = 3,
            Over = 4,
        }


        public RegisterRewardVm(RewardState state, int dayIndex, List<long> rewardConfig)
        {
            State = state;
            DayIndex = dayIndex;
            RewardConfig = rewardConfig;
        }

        /// <summary>
        /// 状态
        ///  None = 0, 没有注册奖励
        /// NotBegin = 1:还未开始
        /// Available = 2,可以领取
        /// Getted = 3,已经被领取
        /// Over = 4,奖励已经结束
        /// </summary>
        public RewardState State { get; set; }
        /// <summary>
        /// 领取的第几天
        /// </summary>
        public int DayIndex { get; set; }
        /// <summary>
        /// 奖励配置, 每一天奖励的金币数量
        /// </summary>
        public List<long> RewardConfig { get; set; }
    }

    /// <summary>
    /// 领取奖励响应
    /// </summary>
    public class RewardInfoVm
    {
        public RewardInfoVm(long rewardCoins)
        {
            RewardCoins = rewardCoins;
        }

        /// <summary>
        /// 领取的金币奖励
        /// </summary>
        public long RewardCoins { get; set; }
    }

    public class LoginRewardVm
    {
        public class OneReward
        {
            public int DayIndex { get; set; }
            public OneRewardState State { get; set; }
            public long RewardCoins { get; set; }
        }
        public enum OneRewardState
        {

            Getted = 0,
            NotGetted = 1,
            Available = 2,
            Waitting = 3,
        }

        public LoginRewardVm(List<OneReward> rewardConfig)
        {
            RewardConfig = rewardConfig;
        }

        /// <summary>
        /// 奖励配置, 每一天奖励的金币数量
        /// </summary>
        public List<OneReward> RewardConfig { get; set; }
    }

    public class BankruptcyInfoVm
    {
        public enum BankruptcyRewardType
        {
            //每天破产补助
            Day,
            //终身破产补助
            lifelong
        }

        public BankruptcyInfoVm(BankruptcyRewardType type, int totalTimes,
            int curTimes, List<long> rewardConfig)
        {
            Type = type;
            TotalTimes = totalTimes;
            CurTimes = curTimes;
            RewardConfig = rewardConfig;
        }

        /// <summary>
        ///每天破产补助
        ///Day,
        ///终身破产补助
        ///lifelong
        /// </summary>
        public BankruptcyRewardType Type { get; set; }
        /// <summary>
        /// 总共几次破产机会
        /// </summary>
        public int TotalTimes { get; set; }
        /// <summary>
        /// 当前是第几次
        /// </summary>
        public int CurTimes { get; set; }
        /// <summary>
        /// 破产几次的配置
        /// </summary>
        /// 
        public List<long> RewardConfig { get; set; }
    }
}
