using System;
using System.Collections.Generic;
using System.Text;

namespace Account.Domain.Entitys
{
    public class MoneyInfo
    {
        public MoneyInfo()
        {
        }

        public MoneyInfo(long curCoins, long curDiamonds, long maxCoins, long maxDiamonds)
        {
            CurCoins = curCoins;
            CurDiamonds = curDiamonds;
            MaxCoins = maxCoins;
            MaxDiamonds = maxDiamonds;
        }

        public long CurCoins { get; private set; }
        public long CurDiamonds { get; private set; }
        public long MaxCoins { get; private set; }
        public long MaxDiamonds { get; private set; }
    }
}
