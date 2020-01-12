using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Commons.Buses.ProcessBus
{
    public abstract class ProcessCommand<TResponse> : IRequest<TResponse>, ICommand
    {
        public DateTime TimeStamp { get; private set; }

        public string CommandType { get; protected set; }

        public Guid AggregateId { get; protected set; }

        //public ValidationResult ValidationResult { get; set; }

        public ProcessCommand()
        {
            CommandType = GetType().Name;

            TimeStamp = DateTime.Now;

            AggregateId = Guid.NewGuid();
        }
    }
}
