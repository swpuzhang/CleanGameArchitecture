using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Commons.Buses.ProcessBus;
using Commons.DiIoc;
namespace Commons.Buses
{
    public interface IMediatorHandler : IDependency
    {
        Task<TResponse> SendCommand<TResponse>(ProcessCommand<TResponse> command);
        Task RaiseEvent<T>(T @event) where T : IEvent;
    }
}
