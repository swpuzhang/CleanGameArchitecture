using Commons.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account.ViewModels
{
    public class AccountDetailVm
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

