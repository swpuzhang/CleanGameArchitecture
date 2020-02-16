using RoomMatch.Domain.Entitys;
using RoomMatch.ViewModels;
using Commons.Buses.ProcessBus;
using Commons.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomMatch.Domain.ProcessCommands
{
    public class RoomMatchCommand : ProcessCommand<WrappedResponse<RoomMatchResponseVm>>
    {
        public RoomMatchInfo Info { get; private set; }
        public RoomMatchCommand(RoomMatchInfo info)
        {
            Info = info;
        }
    }
}
