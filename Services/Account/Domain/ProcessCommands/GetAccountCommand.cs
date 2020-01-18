using Account.ViewModels;
using CommonMessages.MqCmds;
using Commons.Buses.ProcessBus;
using Commons.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account.Domain.ProcessCommands
{
    public class GetSelfAccountCommand : ProcessCommand<WrappedResponse<AccountDetailVm>>
    {
        public long Id { get; private set; }
        public GetSelfAccountCommand(long id)
        {
            Id = id;
        }
    }

    public class GetOtherAccountCommand : ProcessCommand<WrappedResponse<OtherAccountDetailVm>>
    {
        public long Id { get; private set; }
        public long OtherId { get; private set; }
        public GetOtherAccountCommand(long id, long otherId)
        {
            Id = id;
            OtherId = otherId;
        }
    }

    public class GetAccountBaseInfoCommand : ProcessCommand<WrappedResponse<GetAccountBaseInfoMqResponse>>
    {
        public long Id { get; private set; }
        public GetAccountBaseInfoCommand(long id)
        {
            Id = id;
        }
    }


    public class GetIdByPlatformCommand : ProcessCommand<WrappedResponse<GetIdByPlatformMqResponse>>
    {
        public string PlatformAccount { get; private set; }
        public int Type { get; private set; }
        public GetIdByPlatformCommand(string platformAccount, int type)
        {
            PlatformAccount = platformAccount;
            Type = type;
        }
    }
}
