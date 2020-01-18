using System;
using System.Collections.Generic;
using System.Text;

namespace Reward.Domain.Entitys
{
    public class RegisterRewardConfig
    {
        public List<long>  DayRewards { get; set; }


    }

    public class LoginRewardConfig
    {
        public List<long> DayRewards { get; set; }
    }

    public class BankruptcyConfig
    {
        public List<long> BankruptcyRewards { get; set; }
    }

    public class InviteRewardConfig
    {
        public long InviteRewards { get; set; }
    }

    public class DdvertRewardConfig
    {

    }
}
