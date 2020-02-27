using Money.Domain.Entitys;
using Money.ViewModels;
using Commons.Buses.ProcessBus;
using Commons.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Commons.Enums;

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
        public AddMoneyCommand(long id, long addCoins, long addCarry, AddReason reason)
        {
            Id = id;
            AddCoins = addCoins;
            AddCarry = addCarry;
            Reason = reason;
        }

        public long Id { get; private set; }
        public long AddCoins { get; private set; }
        public long AddCarry { get; private set; }
        public AddReason Reason { get; private set; }
    }

    public class BuyInCommand : ProcessCommand<WrappedResponse<MoneyInfo>>
    {
        public BuyInCommand(long id, long minBuy, long maxBuy, AddReason reason)
        {
            Id = id;
            MinBuy = minBuy;
            MaxBuy = maxBuy;
            Reason = reason;
        }

        public long Id { get; private set; }
        public long MinBuy { get; private set; }
        public long MaxBuy { get; private set; }
        public AddReason Reason { get; private set; }
    }
}
