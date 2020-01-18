using CommonMessages.MqCmds;
using Commons.Enums;
using Commons.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Account.Domain.Entitys
{
    public class AccountInfo : UserEntity
    {
        public AccountInfo()
        {

        }
        public string PlatformAccount { get; private set; }

        public string UserName { get; private set; }

        public int Sex { get; private set; }

        public string HeadUrl { get; private set; }

        public AccountType Type { get; private set; }

        public DateTime RegisterDate { get; private set; }

        public GetAccountBaseInfoMqResponse.SomeFlags Flags { get; private set; }

        public void FinishRegister()
        {
            Flags |= GetAccountBaseInfoMqResponse.SomeFlags.RegisterReward;
        }
        [JsonConstructor]
        public AccountInfo(long id, string platformAccount, string userName,
            int sex, string headUrl, AccountType type, DateTime registerDate,
            GetAccountBaseInfoMqResponse.SomeFlags flags = GetAccountBaseInfoMqResponse.SomeFlags.None)
        {

            Id = id;
            PlatformAccount = platformAccount;
            UserName = userName;
            Sex = sex;
            HeadUrl = headUrl;
            Type = type;
            RegisterDate = registerDate;
            Flags = flags;
        }

        public bool IsNeedUpdate(AccountInfo info)
        {
            return UserName != info.UserName &&
                   Sex != info.Sex &&
                   HeadUrl != info.HeadUrl;
        }
    }

    public class AccountResponse
    {

        private AccountResponse()
        {

        }

        public AccountResponse(long id, string platformAccount,
            string userName, int sex, string headUrl,
            string token, MoneyInfo moneyInfo,
            string longConnectHost, bool isRegister, AccountType type)
        {
            Id = id;
            PlatformAccount = platformAccount;
            UserName = userName;
            Sex = sex;
            HeadUrl = headUrl;
            Token = token;
            MoneyInfo = moneyInfo;
            LongConnectHost = longConnectHost;
            IsRegister = isRegister;
            Type = type;
        }

        public Int64 Id { get; private set; }
        public string PlatformAccount { get; private set; }
        public string UserName { get; private set; }
        public int Sex { get; set; }
        public string HeadUrl { get; private set; }
        public AccountType Type { get; private set; }

        public MoneyInfo MoneyInfo { get; private set; }


        public string Token { get; private set; }

        public string LongConnectHost { get; set; }

        public bool IsRegister { get; set; }
    }
}
