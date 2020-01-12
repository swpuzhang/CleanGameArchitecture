using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Commons.Buses.ProcessBus
{
    public class ProcessEvent : INotification, IEvent
    {
        public string EventType { get; set; }

        public Guid AggregateId { get; set; }

        public DateTime Timestamp { get; set; }
        protected ProcessEvent()
        {
            EventType = GetType().Name;
            AggregateId = Guid.NewGuid();
            Timestamp = DateTime.Now;
        }
    }
}
