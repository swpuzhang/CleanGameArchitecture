using Commons.Buses.ProcessBus;
using Commons.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reward.Domain.Events
{
    public class RewardEvent : ProcessEvent
    {
        public RewardEvent()
        {
        }

        public RewardEvent(Guid gid)
        {
            AggregateId = gid;
        }
    }
    public class InvitedFriendEvent : ProcessEvent
    {
        public long Id { get; private set; }
        public string PlatformAccount { get; private set; }
        public AccountType Type { get; private set; }
        public InvitedFriendEvent(long id, string platformAccount, AccountType type)
        {
            Id = id;
            PlatformAccount = platformAccount;
            Type = type;
        }


    }

    public class InvitedFriendRegisterdEvent : ProcessEvent
    {
        public string PlatformAccount { get; private set; }
        public AccountType Type { get; private set; }
        public InvitedFriendRegisterdEvent(string platformAccount, AccountType type)
        {
           
            PlatformAccount = platformAccount;
            Type = type;
        }
    }
}
