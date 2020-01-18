using Account.ViewModels;
using CommonMessages.MqCmds;
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
        Task<WrappedResponse<GetAccountBaseInfoMqResponse>> GetAccountBaseInfo(long id);
        void FinishRegisterReward(long id);
        Task<WrappedResponse<GetIdByPlatformMqResponse>> GetIdByPlatform(string platformAccount, int type);
    }
}
