using Commons.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Commons.MqEvents
{
    public class LoginMqEvent
    {
        public LoginMqEvent()
        {

        }
        public LoginMqEvent(long id, string platformAccount,
            string userName, int sex, string headUrl,
            string token, long actionTime, AccountType type)
        {
            Id = id;
            PlatformAccount = platformAccount;
            UserName = userName;
            Sex = sex;
            HeadUrl = headUrl;
            Token = token;
            ActionTime = actionTime;
            Type = type;
        }

        public Int64 Id { get; set; }
        public string PlatformAccount { get; set; }
        public string UserName { get; set; }
        public int Sex { get; set; }
        public string HeadUrl { get; set; }
        public string Token { get; set; }
        /// <summary>
         /// 账号类型
         /// </summary>
        public AccountType Type { get; set; }
        public long ActionTime { get; set; }
    }

    public class RegistMqEvent : LoginMqEvent
    {
        public RegistMqEvent()
        {

        }
        public RegistMqEvent(long id, string platformAccount,
            string userName, int sex, string headUrl,
            string token, long actionTime, AccountType type)
            
        {
            Id = id;
            PlatformAccount = platformAccount;
            UserName = userName;
            Sex = sex;
            HeadUrl = headUrl;
            Token = token;
            ActionTime = actionTime;
            Type = type;
        }
    }

    public class FinishedRegisterRewardMqEvent
    {
        public FinishedRegisterRewardMqEvent(long id)
        {
            Id = id;
        }

        public long Id { get; private set; }
    }
}
