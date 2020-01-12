using Account.ViewModels;
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
}
