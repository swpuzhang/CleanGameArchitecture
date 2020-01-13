using Commons.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account.ViewModels
{
    /// <summary>
    /// 账号详细信息
    /// </summary>
    public class AccountDetailVm
    {
        /// <summary>
        /// 平台Id
        /// </summary>
        public string PlatformAccount { get; set; }

        /// <summary>
        /// 玩家姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 性别0-男 1-女
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// 头像url
        /// </summary>
        public string HeadUrl { get; set; }

        /// <summary>
        /// 账号类型
        /// </summary>
        public AccountType Type { get; set; }

        /// <summary>
        /// 等级信息
        /// </summary>
        public LevelInfoVm LevelInfo { get; set; }

        /// <summary>
        /// 游戏信息
        /// </summary>
        public GameInfoVm GameInfo { get; set; }

        /// <summary>
        /// 金币信息
        /// </summary>
        public MoneyInfoVm MoneyInfo { get; set; }

        public AccountDetailVm()
        {

        }


        public AccountDetailVm(string platformAccount, string userName,
            int sex, string headUrl, AccountType type,
            LevelInfoVm levelInfo, GameInfoVm gameInfo, MoneyInfoVm moneyInfo = null)
        {
            PlatformAccount = platformAccount;
            UserName = userName;
            Sex = sex;
            HeadUrl = headUrl;
            Type = type;
            LevelInfo = levelInfo;
            GameInfo = gameInfo;
            MoneyInfo = moneyInfo;
        }
    }

    /// <summary>
    /// 其他玩家账号信息
    /// </summary>
    public class OtherAccountDetailVm
    {
        public string PlatformAccount { get; set; }

        public string UserName { get; set; }

        public int Sex { get; set; }

        public string HeadUrl { get; set; }

        /// <summary>
        /// 账号类型
        /// </summary>
        public AccountType Type { get; set; }

        public LevelInfoVm LevelInfo { get; set; }

        public GameInfoVm GameInfo { get; set; }

        public MoneyInfoVm MoneyInfo { get; set; }

        /// <summary>
        /// 好友类型 -1 非好友 0 游戏好友 1 平台好友
        /// </summary>
        public FriendTypes FriendType { get; set; }

        public OtherAccountDetailVm()
        {

        }

        public OtherAccountDetailVm(string platformAccount, string userName,
            int sex, string headUrl, AccountType type,
            LevelInfoVm levelInfo, GameInfoVm gameInfo,
            MoneyInfoVm moneyInfo, FriendTypes friendType)
        {
            PlatformAccount = platformAccount;
            UserName = userName;
            Sex = sex;
            HeadUrl = headUrl;
            Type = type;
            LevelInfo = levelInfo;
            GameInfo = gameInfo;
            MoneyInfo = moneyInfo;
            FriendType = friendType;
        }
    }
}

