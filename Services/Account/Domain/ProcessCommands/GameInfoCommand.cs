using Account.Domain.Entitys;
using Commons.Buses.ProcessBus;
using Commons.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account.Domain.ProcessCommands
{
    public class GetGameInfoCommand : ProcessCommand<WrappedResponse<GameInfo>>
    {
        public long Id { get; private set; }
        public GetGameInfoCommand(long id)
        {
            Id = id;
        }
    }
}
