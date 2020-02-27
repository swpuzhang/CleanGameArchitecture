using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Game.Domain.Entitys;
using Game.ViewModels;
using Commons.Buses.ProcessBus;

namespace Game.Domain.ProcessEvents
{
    public class GameEvent : ProcessEvent
    {
        public GameEvent()
        {
        }

        public GameEvent(Guid gid)
        {
            AggregateId = gid;;
        }

    }
}
