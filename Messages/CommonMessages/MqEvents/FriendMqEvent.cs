
using Commons.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonMessages.MqEvents
{
    public class InviteFriendMqEvent
    {
        public InviteFriendMqEvent()
        {

        }
        public InviteFriendMqEvent(long id, string platformAccount, AccountType type)
        {
            Id = id;
            PlatformAccount = platformAccount;
            Type = type;
        }
        /// <summary>
        /// 邀请的玩家ID
        /// </summary>
        public Int64 Id { get; set; }
        /// <summary>
        /// 被邀请的平台账号
        /// </summary>
        public string PlatformAccount { get; set; }
        /// <summary>
        /// 被邀请的平台类型
        /// </summary>
        public AccountType Type { get; set; }
    }

    /// <summary>
    /// 请求添加好友事件
    /// </summary>
    public class ApplyedAddFriendMqEvent
    {
        [JsonConstructor]
        public ApplyedAddFriendMqEvent(long applyId)
        {
            ApplyId = applyId;
        }

        /// <summary>
        /// 请求Id
        /// </summary>
        public Int64 ApplyId { get; set; }
    }

}
