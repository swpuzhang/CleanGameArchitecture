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
    public class PlaynowCommand : ProcessCommand<WrappedResponse<RoomMatchResponseVm>>
    {
        public long Id { get; private set; }
        public PlaynowCommand(long id)
        {
            Id = id;
        }
    }

    public class BlindMatchCommand : ProcessCommand<WrappedResponse<RoomMatchResponseVm>>
    {
        public long Id { get; private set; }
        public long Blind { get; private set; }
        public BlindMatchCommand(long id, long blind)
        {
            Id = id;
            Blind = blind;
        }
    }
}
