using Commons.Buses.ProcessBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account.Domain.ProcessEvents
{
    public class FinishRegisterRewardEvent : ProcessEvent
    {
        public long Id { get; private set; }
        public FinishRegisterRewardEvent(long id)
        {
            Id = id;
        }
    }
}
