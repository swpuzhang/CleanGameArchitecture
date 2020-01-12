using Money.ViewModels;
using Commons.DiIoc;
using Commons.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Messages.MqCmds;

namespace Money.Application.Services
{
    public interface IMoneyService : IDependency
    {
        public Task<WrappedResponse<MoneyMqResponse>> GetMoney(long id);
        public Task<WrappedResponse<MoneyMqResponse>> AddMoney(long id, long addCoins, long addCarry);
    }
}
