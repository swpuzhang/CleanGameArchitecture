using Commons.Buses.ProcessBus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using Commons.Rpc;
namespace Commons.Buses.MqBus
{
    public static class MassTrasitExtenssion
    {
        public static Task<Response<T>> GetResponseExt<TRequest, T>(this IRequestClient<TRequest> client, TRequest message,
            int msTimeout = 3000) where T : class
            where TRequest : class
        {
            return client.GetResponse<T>(message, timeout: RequestTimeout.After(0, 0, 0, 0, msTimeout));
        }
    }

    public static class RabbitMqBus
    {
        /*public static Task RaiseEvent<T>(this IBus bus, T ev) where T: class
        {
            return bus.Publish(ev);
        }

        public static Task SendCommand<T>(this IBus bus, T command) where T : class
        {
            return bus.Publish(command);
        }

        public static Task<TResponse> RecvResponse<T, TResponse>(this IBus bus, T command) where T : class
        {
            return RpcCaller.RpcCallAsync(() => bus.Publish(command));
        }*/
    }
}
