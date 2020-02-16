using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RoomMatch.Domain.Entitys;
using RoomMatch.ViewModels;
using Commons.Buses.ProcessBus;

namespace RoomMatch.Domain.ProcessEvents
{
    public class RoomMatchEvent : ProcessEvent
    {
        public RoomMatchEvent()
        {
        }

        public RoomMatchEvent(Guid gid)
        {
            AggregateId = gid;;
        }

    }
}
