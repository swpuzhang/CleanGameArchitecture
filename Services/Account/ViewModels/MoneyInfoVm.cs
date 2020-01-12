using System;
using System.Collections.Generic;
using System.Text;

namespace Account.ViewModels
{ 
    public class MoneyInfoVm
    {
        public MoneyInfoVm()
        {
        }

        public MoneyInfoVm(long curCoins, long curDiamonds, long maxCoins, long maxDiamonds)
        {
            CurCoins = curCoins;
            CurDiamonds = curDiamonds;
            MaxCoins = maxCoins;
            MaxDiamonds = maxDiamonds;
        }

        public long CurCoins { get; set; }
        public long CurDiamonds { get; set; }
        public long MaxCoins { get; set; }
        public long MaxDiamonds { get; set; }
    }
}
