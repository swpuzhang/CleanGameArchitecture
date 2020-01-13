using Commons.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Messages.MqEvents
{
    public class MoneyChangedMqEvent
    {
        public MoneyChangedMqEvent(long coinsChangeCount, long diamondsChangeCount, AddReason reason)
        {
            CoinsChangeCount = coinsChangeCount;
            DiamondsChangeCount = diamondsChangeCount;
            Reason = reason;
        }

        public MoneyChangedMqEvent(long id, long curCoins, long curDiamonds,
            long maxCoins, long maxDiamonds,
            long coinsChangeCount, long diamondsChangeCount,
            AddReason reason)
        {
            Id = id;
            CurCoins = curCoins;
            CurDiamonds = curDiamonds;
            MaxCoins = maxCoins;
            MaxDiamonds = maxDiamonds;
            CoinsChangeCount = coinsChangeCount;
            DiamondsChangeCount = diamondsChangeCount;
            Reason = reason;
        }

        public long Id { get; set; }
        public long CurCoins { get; set; }
        public long CurDiamonds { get; set; }
        public long MaxCoins { get; set; }
        public long MaxDiamonds { get; set; }
        /// <summary>
        /// 变化的金币数量
        /// </summary>
        public long CoinsChangeCount { get; set; }
        /// <summary>
        /// 变化的砖石数量
        /// </summary>
        public long DiamondsChangeCount { get; set; }
        public AddReason Reason { get; set; }

    }
}
