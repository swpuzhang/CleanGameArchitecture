using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Account.Domain.Entitys;
using Account.ViewModels;
using Commons.Buses.ProcessBus;

namespace Account.Domain.ProcessEvents
{
    public class LoginEvent : ProcessEvent
    {
        public LoginEvent()
        {
        }

        public LoginEvent(Guid gid, AccountResponse accounResponse, bool isRegister, bool isNeedUpdate, AccountInfo info)
        {
            AggregateId = gid;
            AccounResponse = accounResponse;
            IsRegister = isRegister;
            IsNeedUpdate = isNeedUpdate;
            Info = info;
        }

        public AccountResponse AccounResponse { get; set; }
        public bool IsRegister { get; set; }

        public bool IsNeedUpdate { get; private set; }
        public AccountInfo Info { get; private set; }
    }
}
