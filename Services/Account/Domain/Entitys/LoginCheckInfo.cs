using Commons.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Account.Domain.Entitys
{
    public class LoginCheckInfo
    {
        LoginCheckInfo()
        {

        }
        [JsonConstructor]
        public LoginCheckInfo(long id, string platformAccount, AccountType type)
        {
            Id = id;
            PlatformAccount = platformAccount;
            Type = type;
        }
        public long Id { get; private set; }
        public string PlatformAccount { get; private set; }
        public AccountType Type { get; private set; }
    }
}
