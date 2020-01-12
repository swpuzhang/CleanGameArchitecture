using Commons.Models;
using System;
using System.Threading.Tasks;

namespace Commons.Rpc
{
    public interface IRpcCaller
    {
        Task<WrappedResponse<NullBody>> RpcCallAsync(Action c, int waitMiliSeconds = 5000);
        void OnResponse(Guid id);
    }
}
