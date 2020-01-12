using System;
using System.Collections.Generic;
using System.Text;

namespace Commons.Enums
{
    /// <summary>
    /// 响应状态码
    /// </summary>
    public enum ResponseStatus
    {
        /// <summary>
        /// 通用错误
        /// </summary>
        Error = -1,
        /// <summary>
        /// 成功
        /// </summary>
        Success = 0,
        /// <summary>
        /// 登录失败
        /// </summary>
        LoginError = 1,
        /// <summary>
        /// 字段错误
        /// </summary>
        FieldError = 2,
        /// <summary>
        /// 总线请求错误
        /// </summary>
        BusError = 3,
        /// <summary>
        /// 超时
        /// </summary>
        Timeout =4,
        
        /// <summary>
        /// 客户端已经断开连接
        /// </summary>
        AppIsDisconnected = 5,

        /// <summary>
        /// GUID错误， 可能碰到重复
        /// </summary>
        GuidError = 6,
        /// <summary>
        /// 验证token失败
        /// </summary>
        TokenError = 7,

        /// <summary>
        /// token过期, 重新登录
        /// </summary>
        TokenExpiredPleaseRelogin =8,

        /// <summary>
        /// huoq
        /// </summary>
        GetMoneyError = 9,

        AccountError = 10,


        GameInfoError = 11,

        IsMatching = 12,

        /// <summary>
        /// 钱不够
        /// </summary>
        NoEnoughMoney = 13,

        PlayerNotInRoom = 14,

        RewardGetted = 15,

        RewardNotAvailable = 16,

        /// <summary>
        /// 已经是好友
        /// </summary>
        IsAlreadyFriend = 17,
        /// <summary>
        /// 已经申请
        /// </summary>
        IsAlreadyApplyed = 18,
        /// <summary>
        /// 已经满了, 好友数量, 申请数量
        /// </summary>
        IsFull = 19,
        /// <summary>
        /// 该好友没有申请
        /// </summary>
        IsNotApplyed = 20,
    }
}
