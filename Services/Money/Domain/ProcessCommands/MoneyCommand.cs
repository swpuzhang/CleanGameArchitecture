using Money.Domain.Entitys;
using Money.ViewModels;
using Commons.Buses.ProcessBus;
using Commons.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Money.Domain.ProcessCommands
{
    public class GetMoneyCommand : ProcessCommand<WrappedResponse<MoneyInfo>>
    {
        public long Id { get; private set; }
        public GetMoneyCommand(long id)
        {
            Id = id;
        }
    }

    public class AddMoneyCommand : ProcessCommand<WrappedResponse<MoneyInfo>>
    {
        public AddMoneyCommand(long id, long addCoins, long addCarry)
        {
            Id = id;
            AddCoins = addCoins;
            AddCarry = addCarry;
        }

        public long Id { get; private set; }
        public long AddCoins { get; private set; }
        public long AddCarry { get; private set; }

    }
}
