using Commons.Enums;
using Commons.Models;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Reward.Domain.Entitys
{
    public class RegisterRewardInfo : UserEntity
    {
        public int DayIndex { get; private set; }
        public DateTime GetDate { get; private set; }
        [JsonConstructor]
        public RegisterRewardInfo(long id, int dayIndex, DateTime getDate)
        {
            Id = id;
            DayIndex = dayIndex;
            GetDate = getDate;
        }
    }

    public class LoginRewardInfo : UserEntity
    {
        [JsonConstructor]
        public LoginRewardInfo(long id,  List<int> gettedDays)
            
        {
            Id = id;
            GettedDays = gettedDays;
        }

        public List<int> GettedDays { get; private set; }
    }


    public class BankruptcyInfo : UserEntity
    {
        [JsonConstructor]
        public BankruptcyInfo(int curTimes)
        {
            CurTimes = curTimes;
        }

        public int CurTimes { get; set; }
    }

   

    public class InviteRewardInfo : UserEntity
    {
        [JsonConstructor]
        public InviteRewardInfo(int curTimes)
        {
            CurTimes = curTimes;
        }

        public int CurTimes { get; set; }
    }
}
