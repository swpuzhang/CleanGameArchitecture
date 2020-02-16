using MassTransit;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Commons.Threading
{
    public abstract class OneThreadConsumer<TRequest, TResponse> : IConsumer<TRequest>
        where TRequest : class
        where TResponse : class
    {
        public async Task Consume(ConsumeContext<TRequest> context)
        {
            TaskCompletionSource<TResponse> methodCallCompletionSource = new TaskCompletionSource<TResponse>();
            OneThreadSynchronizationContext.Instance.Post(async x => {
                try
                {
                    methodCallCompletionSource.SetResult(await ConsumerHandler(context.Message));
                }
                catch (Exception ex)
                {
                    methodCallCompletionSource.SetException(ex);
                }

                
            }, null);
            var response = await methodCallCompletionSource.Task;
            await context.RespondAsync<TResponse>(response);
        }
        public abstract Task<TResponse> ConsumerHandler(TRequest request);
    }

    public abstract class OneThreadConsumer<TRequest> : IConsumer<TRequest> where TRequest : class
    {
        public async Task Consume(ConsumeContext<TRequest> context)
        {
            TaskCompletionSource<int> methodCallCompletionSource = new TaskCompletionSource<int>();
            OneThreadSynchronizationContext.Instance.Post(x => {
                try
                {
                    ConsumerHandler(context.Message);
                    methodCallCompletionSource.SetResult(0);
                }
                catch(Exception ex)
                {
                    methodCallCompletionSource.SetException(ex);
                }
              
            }, null);
            await methodCallCompletionSource.Task;
        }
        public abstract void ConsumerHandler(TRequest request);
    }

    public class OneThreadSynchronizationContext : SynchronizationContext
	{
		public static OneThreadSynchronizationContext Instance { get; } = new OneThreadSynchronizationContext();

		private readonly int mainThreadId = Thread.CurrentThread.ManagedThreadId;

        BlockingCollection<Action> _queue = new BlockingCollection<Action>();

        //private Action a;

        public static async Task<TResponse> UserRequest<T, TResponse>(T p1, Func<T, Task<TResponse>> fuc)  where TResponse:class
        {
            TaskCompletionSource<TResponse> methodCallCompletionSource = new TaskCompletionSource<TResponse>();
            OneThreadSynchronizationContext.Instance.Post(async x => {
                try
                {
                    var innnerresponse = await fuc(p1);
                    methodCallCompletionSource.SetResult(innnerresponse);
                }
                catch (Exception ex)
                {
                    methodCallCompletionSource.TrySetException(ex);
                }
                
               
            }, null);
            var response = await methodCallCompletionSource.Task;
            return response;
        }

        public static async Task<TResponse> UserRequest<T1, T2, TResponse>(T1 p1, T2 p2, Func<T1, T2, Task<TResponse>> fuc) where TResponse : class
        {
            TaskCompletionSource<TResponse> methodCallCompletionSource = new TaskCompletionSource<TResponse>();
            OneThreadSynchronizationContext.Instance.Post(async x => {
                try
                {
                    var innnerresponse = await fuc(p1, p2);
                    methodCallCompletionSource.SetResult(innnerresponse);
                }
                catch (Exception ex)
                {
                    methodCallCompletionSource.TrySetException(ex);
                }


            }, null);
            var response = await methodCallCompletionSource.Task;
            return response;
        }

        public void Update()
		{
            foreach (var one in _queue.GetConsumingEnumerable())
            {
                one();
            }
        }

		public override void Post(SendOrPostCallback callback, object state)
		{
            if (Thread.CurrentThread.ManagedThreadId == this.mainThreadId)
            {
                callback(state);
                return;
            }
            _queue.Add(() => { callback(state); });
        }

        public static void Run()
        {
            SynchronizationContext.SetSynchronizationContext(OneThreadSynchronizationContext.Instance);
            OneThreadSynchronizationContext.Instance.Update();        }
    }
}
