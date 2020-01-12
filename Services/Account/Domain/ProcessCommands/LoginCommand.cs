using Account.Domain.Entitys;
using Account.ViewModels;
using Commons.Buses.ProcessBus;
using Commons.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account.Domain.ProcessCommands
{
    public class LoginCommand : ProcessCommand<WrappedResponse<AccountResponse>>
    {
        public AccountInfo Info { get; private set; }
        public LoginCommand(AccountInfo info)
        {
            Info = info;
        }
    }
}
