using System;
using System.Collections.Generic;
using System.Text;

namespace Commons.Rpc
{
    public interface IRpcResponse
    {
        public Guid Id { get; }
    }
}
