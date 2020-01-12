using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Commons.Models;
using Commons.Enums;

namespace Commons.Rpc
{
    public static class RpcCaller
    {
       // private readonly ConcurrentDictionary<Guid, KeyValuePair<TaskCompletionSource<BodyResponse<NullBody>>, Timer>> _pendingMethodCalls = 
        //    new ConcurrentDictionary<Guid, KeyValuePair<TaskCompletionSource<BodyResponse<NullBody>>, Timer>>();

        private static readonly ConcurrentDictionary
            <Guid,KeyValuePair<TaskCompletionSource<WrappedResponse<NullBody>>,CancellationTokenSource>> _Calls =
            new ConcurrentDictionary<Guid, KeyValuePair<TaskCompletionSource<WrappedResponse<NullBody>>, CancellationTokenSource>>();

        public static void OnResponse<TResponse>(TResponse res)  where TResponse : IRpcResponse
        {
            if (_Calls.TryRemove(res.Id, out var value))
            {
                value.Key.TrySetResult(new WrappedResponse<NullBody>(ResponseStatus.Success, null));
                value.Value.Dispose();
            }
        }

        public static Task<WrappedResponse<NullBody>> RpcCallAsync(Action c, int waitMiliSeconds = 5000)
        {
            Guid id = Guid.NewGuid();
            TaskCompletionSource<WrappedResponse<NullBody>> methodCallCompletionSource = new TaskCompletionSource<WrappedResponse<NullBody>>();
            CancellationTokenSource token = new CancellationTokenSource(waitMiliSeconds);
            if (_Calls.TryAdd(id, new KeyValuePair<TaskCompletionSource<WrappedResponse<NullBody>>,
                CancellationTokenSource>(methodCallCompletionSource, token)))
            {
                try
                {
                    using (token.Token.Register(
                        () =>
                        {
                            if (_Calls.TryRemove(id, out var value))
                            {
                                value.Key.TrySetException(new Exception("timeout"));
                                //value.Key.TrySetResult(new WrappedResponse<NullBody>(ResponseStatus.Timeout, new List<string>() { "timeout" }));
                                value.Value.Dispose();
                            }
                        }
                        ))
                    {
                        Task.Run(() =>
                        {
                            try
                            {
                                c();
                            }
                            catch (Exception ex)
                            {
                                if (_Calls.TryRemove(id, out var value))
                                {
                                    value.Key.TrySetException(ex);
                                    //value.Key.TrySetResult(new WrappedResponse<NullBody>(ResponseStatus.Error, new List<string>() { ex.Message}));
                                    value.Value.Dispose();
                                }
                            }
                        });
                        return methodCallCompletionSource.Task;
                    }
                }

                catch (Exception ex)
                {
                    if (_Calls.TryRemove(id, out var value))
                    {
                        value.Value.Dispose();
                    }
                    throw ex;
                }
            }
            else
            {
                token.Dispose();
                throw new Exception("GuidError");
            }
        }

        
    }
}
