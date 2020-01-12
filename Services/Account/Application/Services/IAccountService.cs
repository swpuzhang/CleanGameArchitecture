using Account.ViewModels;
using Commons.DiIoc;
using Commons.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account.Application.Services
{
    public interface IAccountService : IDependency
    {
        public Task<WrappedResponse<AccountResponseVm>> Login(AccountInfoVm accountInfo);
        Task<WrappedResponse<AccountDetailVm>> GetSelfAccount(long id);
        Task<WrappedResponse<OtherAccountDetailVm>> GetOtherAccount(long id, long otherId);
    }
}
