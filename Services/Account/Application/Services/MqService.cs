using Account.Domain.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account.Application.Services
{
    public class MqService : IMqService
    {

        public void NotifyHostInfo(string host, int userCount)
        {
            WSHostManager.OnNotifyHostInfo(host, userCount);
        }
    }
}
