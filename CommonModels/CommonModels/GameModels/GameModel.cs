using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonModels.GameModels
{
    public class PokerCard : IComparable<PokerCard>
    {

        [JsonConstructor]
        public PokerCard(int point, int color)
        {
            Point = point;
            Color = color;
        }

        public PokerCard()
        {
        }

        public void SetCard(int point, int color)
        {
            Point = point;
            Color = color;
        }

        public int CompareTo(PokerCard other)
        {
            int ret = Point.CompareTo(other.Point);
            if (ret == 0)
            {
                return Color.CompareTo(other.Color);
            }
            return ret;
        }

        public int CompareFlush(PokerCard other)
        {
            int ret = Color.CompareTo(other.Color);
            if (ret == 0)
            {
                return Point.CompareTo(other.Point);
            }
            return ret;
        }

        public int Point { get; set; }
        public int Color { get; set; }
    }

    /// <summary>
    /// 奖池
    /// </summary>
    public class WinnerCoinsPool
    {
        [JsonConstructor]
        public WinnerCoinsPool(int winnerSeat, long coins)
        {
            WinnerSeat = winnerSeat;
            Coins = coins;
        }

        /// <summary>
        /// 该奖池的赢家座位号
        /// </summary>
        public int WinnerSeat { get; set; }

        /// <summary>
        /// 该奖池的金币数
        /// </summary>
        public long Coins { get; set; }
    }


    public class PlayerInfo
    {
        /// <summary>
        /// 玩家状态， 在牌局中进行判断， 牌局未开始终未0
        /// </summary>
        public enum PlayerStatus
        {
            Idle = 0,
            Watching = 1,
            Playing = 2,
            Drop = 3,
            Allin = 4,

        }

        public PlayerInfo()
        {
        }

        [JsonConstructor]
        public PlayerInfo(long id, int seatNum, string name, long carry,
            string headUrl, int handCardCount, PlayerStatus status,
            List<PokerCard> handCards, int cardType, int points,
            long betCoins)
        {
            Id = id;
            SeatNum = seatNum;
            Name = name;
            Carry = carry;
            HeadUrl = headUrl;
            HandCardCount = handCardCount;
            Status = status;
            HandCards = handCards;
            CardType = cardType;
            Points = points;
            BetCoins = betCoins;
        }

        public long Id { get; set; }
        public int SeatNum { get; set; }
        public string Name { get; set; }
        public long Carry { get; set; }
        public string HeadUrl { get; set; }
        public int HandCardCount { get; set; }
        public PlayerStatus Status { get; set; }
        /// <summary>
        /// 自己的手牌
        /// </summary>
        public List<PokerCard> HandCards { get; set; }
        /// <summary>
        /// 手牌类型
        /// </summary>
        public int CardType { get; set; }

        /// <summary>
        /// 如果是点数牌， 点数
        /// </summary>
        public int Points { get; set; }

        /// <summary>
        /// 当前已经下注额度
        /// </summary>
        public long BetCoins { get; set; }

    }

}
