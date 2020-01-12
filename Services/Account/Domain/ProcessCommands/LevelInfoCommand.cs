using Account.Domain.Entitys;
using Commons.Buses.ProcessBus;
using Commons.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account.Domain.ProcessCommands
{
    public class GetLevelInfoCommand : ProcessCommand<WrappedResponse<LevelInfo>>
    {
        public long Id { get; private set; }
        public GetLevelInfoCommand(long id)
        {
            Id = id;
        }
    }
}
