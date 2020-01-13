using Commons.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account.ViewModels
{
    /// <summary>
    /// 玩家账号
    /// </summary>
    public class AccountInfoVm
    {
        public AccountInfoVm()
        {

        }
        /// <summary>
        /// 平台账号
        /// </summary>
        public string PlatformAccount { get; set; }

        /// <summary>
        /// 平台账号名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 性别0 男， 1女
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// 平台头像url
        /// </summary>
        public string HeadUrl { get; set; }

        /// <summary>
        /// 账号类型
        /// </summary>
        public AccountType Type { get; set; }

    }

    /// <summary>
    /// 请求账号的响应消息
    /// </summary>
    public class AccountResponseVm
    {
        public AccountResponseVm()
        {

        }
        public AccountResponseVm(long id, string platformAccount,
            string userName, int sex, string headUrl, AccountType type,
            string token, long curCoins,
            long curDiamonds, string longConnectHost, bool isRegister)
        {
            Id = id;
            PlatformAccount = platformAccount;
            UserName = userName;
            Sex = sex;
            HeadUrl = headUrl;
            Token = token;
            CurCoins = curCoins;
            CurDiamonds = curDiamonds;
            Type = type;
            LongConnectHost = longConnectHost;
            IsRegister = isRegister;
        }
        /// <summary>
        ///玩家唯一Id 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 平台账号
        /// </summary>
        public string PlatformAccount { get; set; }

        /// <summary>
        /// 平台账号名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 性别0 男， 1女
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// 平台头像url
        /// </summary>
        public string HeadUrl { get; set; }

        /// <summary>
        /// 账号类型
        /// </summary>
        public AccountType Type { get; set; }

        /// <summary>
        /// 当前金币
        /// </summary>
        public long CurCoins { get; set; }

        /// <summary>
        /// 当前砖石数
        /// </summary>
        public long CurDiamonds { get; set; }
        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 长连接地址
        /// </summary>
        public string LongConnectHost { get; set; }
        /// <summary>
        /// 是否是注册，还是普通登录
        /// </summary>
        public bool IsRegister { get; set; }
    }
}
