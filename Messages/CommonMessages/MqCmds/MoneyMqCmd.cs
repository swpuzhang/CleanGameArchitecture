using Commons.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonMessages.MqCmds
{
    public class GetMoneyMqCmd
    {
        public GetMoneyMqCmd(long id)
        {
            Id = id;
        }

        public long Id { get; private set; }
    }

    public class MoneyMqResponse
    {
        public MoneyMqResponse(long id, long curCoins, long curDiamonds,
            long maxCoins, long maxDiamonds, long carry)
        {
            Id = id;
            CurCoins = curCoins;
            CurDiamonds = curDiamonds;
            MaxCoins = maxCoins;
            MaxDiamonds = maxDiamonds;
            Carry = carry;
        }

        public long Id { get; private set; }
        public long CurCoins { get; private set; }
        public long CurDiamonds { get; private set; }
        public long MaxCoins { get; private set; }
        public long MaxDiamonds { get; private set; }
        public long Carry { get; private set; }
    }

    public class AddMoneyMqCmd
    {
        public AddMoneyMqCmd(long id, long addCoins, long addCarry, AddReason reason)
        {
            Id = id;
            AddCoins = addCoins;
            AddCarry = addCarry;
            Reason = reason;
        }

        public long Id { get; private set; }
        public long AddCoins { get; private set; }
        public long AddCarry { get; private set; }
        public AddReason Reason;
    }

    public class BuyInMqCmd
    {
        public BuyInMqCmd(long id, long minBuy, long maxBuy, AddReason reason)
        {
            Id = id;
            MinBuy = minBuy;
            MaxBuy = maxBuy;
            Reason = reason;
        }

        public long Id { get; private set; }
        public long MinBuy { get; private set; }
        public long MaxBuy { get; private set; }
        public AddReason Reason;
    }
}
