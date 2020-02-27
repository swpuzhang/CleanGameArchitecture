using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account.Application.Services
{
    public interface IMqService
    {
        void NotifyHostInfo(string host, int userCount);
    }
}
