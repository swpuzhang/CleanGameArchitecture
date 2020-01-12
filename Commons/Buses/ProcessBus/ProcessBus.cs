using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Commons.Buses.ProcessBus
{
    class ProcessBus : IMediatorHandler
    {
        private readonly IMediator _mediator;

        public ProcessBus(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<TResponse> SendCommand<TResponse>(ProcessCommand<TResponse> command)
        {
            return await _mediator.Send(command);
        }

        public Task RaiseEvent<T>(T @event) where T : IEvent
        {
            //if (!@event.MessageType.Equals("DomainNotification"))
            //    _eventStore?.Save(@event);

            return _mediator.Publish(@event);
        }
    }
}
