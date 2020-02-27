using CommonModels.GameModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameMessages.RoomMessages
{
    public static class RoomMessage
    {
        public static string MessageNameSpace = typeof(PlayerSeatedEvent).Namespace;
    }

    /// <summary>
    /// 玩家坐下广播事件
    /// </summary>
    public class PlayerSeatedEvent
    {
        public PlayerSeatedEvent()
        {
        }

        [JsonConstructor]
        public PlayerSeatedEvent(long userId, string userName, long curCoins, long curDiamonds, int seatNum, long carry)
        {
            UserId = userId;
            UserName = userName;
            CurCoins = curCoins;
            CurDiamonds = curDiamonds;
            SeatNum = seatNum;
            Carry = carry;
        }

        public long UserId { get; set; }
        public string UserName { get; set; }

        public long CurCoins { get; set; }

        public long CurDiamonds { get; set; }

        public int SeatNum { get; set; }

        /// <summary>
        /// 牌桌买入
        /// </summary>
        public long Carry { get; set; }
    }


    /// <summary>
    /// 广播发牌事件
    /// </summary>
    public class DealCardsEvent
    {
        public DealCardsEvent()
        {
        }

        [JsonConstructor]
        public DealCardsEvent(long dealerSeatNum, List<int> dealCardNum, int cardNum,
            List<PokerCard> cards, long blind, List<long> carrys)
        {
            DealerSeatNum = dealerSeatNum;
            DealCardNum = dealCardNum;
            CardNum = cardNum;
            Cards = cards;
            Blind = blind;
            this.Carrys = carrys;
        }

        /// <summary>
        /// 庄家的座位号
        /// </summary>
        public long DealerSeatNum { get; set; }

        /// <summary>
        /// 发牌顺序，从庄家开始的座位号
        /// </summary>
        public List<int> DealCardNum { get; set; }

        /// <summary>
        /// 手牌数量
        /// </summary>
        public int CardNum { get; set; }

        /// <summary>
        /// 自己的手牌
        /// </summary>
        public List<PokerCard> Cards { get; set; }

        /// <summary>
        /// 下底注
        /// </summary>
        public long Blind { get; set; }

        /// <summary>
        /// 按发牌顺序,各个玩家的当前携带
        /// </summary>
        public List<long> Carrys { get; set; }
    }

    [Flags]
    public enum ActiveOperation
    {
        None = 0,
        Follow = 1,
        Drop = 2,
        Add = 3,
    }

    public class ActiveEvent
    {
        public ActiveEvent()
        {
        }

        [JsonConstructor]
        public ActiveEvent(int activeSeatNum, long addCoins)
        {
            ActiveSeatNum = activeSeatNum;
            AddCoins = addCoins;
        }


        /// <summary>
        /// 激活玩家座位号
        /// </summary>
        public int ActiveSeatNum { get; set; }

        /// <summary>
        /// 跟牌显示筹码数, 如果为0 表示过牌
        /// </summary>
        public long AddCoins { get; set; }
    }

    public class DealThirdCardEvent
    {
        public DealThirdCardEvent()
        {
        }

        [JsonConstructor]
        public DealThirdCardEvent(List<long> coinPool, PokerCard card, List<int> order, int cardType, int point)
        {
            CoinPool = coinPool;
            Card = card;
            Order = order;
            CardType = cardType;
            Point = point;
        }

        /// <summary>
        /// 下注分堆， 每一堆多少jinbi
        /// </summary>
        public List<long> CoinPool { get; set; }

        /// <summary>
        /// 第三张牌
        /// </summary>
        public PokerCard Card { get; set; }

        /// <summary>
        /// 三张牌组成的类型
        /// </summary>
        public int CardType { get; set; }

        /// <summary>
        /// 如果类型是点数牌， 点数
        /// </summary>
        public int Point { get; set; }

        /// <summary>
        /// 发牌顺序
        /// </summary>
        public List<int> Order { get; set; }
    }

    /// <summary>
    /// 玩家弃牌事件
    /// </summary>
    public class DropEvent
    {
        public DropEvent()
        {
        }

        [JsonConstructor]
        public DropEvent(int dropSeatNum)
        {
            DropSeatNum = dropSeatNum;
        }


        /// <summary>
        /// 激活玩家座位号
        /// </summary>
        public int DropSeatNum { get; set; }
    }

    public class PassEvent
    {
        public PassEvent()
        {
        }

        [JsonConstructor]
        public PassEvent(int seatNum)
        {
            SeatNum = seatNum;
        }


        /// <summary>
        /// 激活玩家座位号
        /// </summary>
        public int SeatNum { get; set; }
    }

    public class FollowEvent
    {
        public FollowEvent()
        {
        }

        [JsonConstructor]
        public FollowEvent(int seatNum, long followCoins, long carry)
        {
            SeatNum = seatNum;
            FollowCoins = followCoins;
            Carry = carry;
        }

        /// <summary>
        /// 激活玩家座位号
        /// </summary>
        public int SeatNum { get; set; }
        public long FollowCoins { get; set; }

        /// <summary>
        /// 当前携带为0表示allin
        /// </summary>
        public long Carry { get; set; }
    }

    public class AddEvent
    {
        public AddEvent()
        {
        }

        [JsonConstructor]
        public AddEvent(int seatNum, long addCoins, long carry)
        {
            SeatNum = seatNum;
            AddCoins = addCoins;
            Carry = carry;
        }

        /// <summary>
        /// 激活玩家座位号
        /// </summary>
        public int SeatNum { get; set; }
        public long AddCoins { get; set; }

        /// <summary>
        /// 当前携带为0表示allin
        /// </summary>
        public long Carry { get; set; }
    }

    public class PlayerGameOverInfo
    {
        [JsonConstructor]
        public PlayerGameOverInfo(long id, int seatNum,
            long coinsInc, long carry, List<PokerCard> cards,
            int cardType, int point)
        {
            Id = id;
            SeatNum = seatNum;
            CoinsInc = coinsInc;
            Carry = carry;
            Cards = cards;
            CardType = cardType;
            Point = point;
        }

        public long Id { get; set; }

        public int SeatNum { get; set; }

        public long CoinsInc { get; set; }

        public long Carry { get; set; }

        /// <summary>
        /// 手牌
        /// </summary>
        public List<PokerCard> Cards { get; set; }

        /// <summary>
        /// 三张牌组成的类型
        /// </summary>
        public int CardType { get; set; }

        /// <summary>
        /// 如果类型是点数牌， 点数
        /// </summary>
        public int Point { get; set; }

    }

    /// <summary>
    /// 游戏结算
    /// </summary>
    public class GameOverEvent
    {
        [JsonConstructor]
        public GameOverEvent(List<int> winnerSeats, List<WinnerCoinsPool> winnerPool, List<PlayerGameOverInfo> playerInfos)
        {
            WinnerSeats = winnerSeats;
            WinnerPool = winnerPool;
            PlayerInfos = playerInfos;
        }

        /// <summary>
        /// 赢家座位号
        /// </summary>
        public List<int> WinnerSeats { get; set; }

        /// <summary>
        /// 每个奖池
        /// </summary>
        public List<WinnerCoinsPool> WinnerPool { get; set; }

        /// <summary>
        /// 结算每个玩家信息
        /// </summary>
        public List<PlayerGameOverInfo> PlayerInfos { get; set; }
    }

    public class PlayerStanupEvent
    {
        [JsonConstructor]
        public PlayerStanupEvent(long userId, int seatNum)
        {
            UserId = userId;
            SeatNum = seatNum;
        }

        public long UserId { get; set; }
        public int SeatNum { get; set; }
    }

    public class ApplyStandupCommand
    {

    }


    public class ApplyLeaveCommand
    {

    }

    public class ApplySitdownCommand
    {
        [JsonConstructor]
        public ApplySitdownCommand(int seatNum)
        {
            SeatNum = seatNum;
        }

        public int SeatNum { get; set; }

    }

    public class ApplyDropCommand
    {

    }

    public class ApplyPassCommand
    {

    }
    public class ApplyFollowCommand
    {

    }

    public class ApplyAddCommand
    {
        [JsonConstructor]
        public ApplyAddCommand(long addCoins)
        {
            this.AddCoins = addCoins;
        }

        public long AddCoins { get; set; }
    }

    public class ApplyStayInRoom
    {

    }

    public class PlayerBuyInEvent
    {

        [JsonConstructor]
        public PlayerBuyInEvent(int seatNum, long carry)
        {
            SeatNum = seatNum;
            Carry = carry;
        }

        public int SeatNum { get; set; }
        public long Carry { get; set; }
    }

    public class ApplySyncGameRoomCommand
    {

    }

    public class ApplySyncGameRoomResponse
    {
        public enum GameStatus
        {
            /// <summary>
            /// 空闲状态，处于这个状态什么都不用做
            /// </summary>
            Idle = 0,
            /// <summary>
            /// 等待玩家操作， 显示倒计时，
            /// </summary>
            PlayerOpt,
            /// <summary>
            /// 结束状态， 等待下一局开始
            /// </summary>
            GameOver,
        }

        public ApplySyncGameRoomResponse()
        {
        }

        [JsonConstructor]
        public ApplySyncGameRoomResponse(GameStatus status, List<PlayerInfo> players,
            List<long> pools, int timeLeftMs, int playerOptMs, int gameOverMs)
        {
            Status = status;
            Players = players;
            Pools = pools;
            TimeLeftMs = timeLeftMs;
            PlayerOptMs = playerOptMs;
            GameOverMs = gameOverMs;
        }




        /// <summary>
        /// 牌桌状态 
        /// Idle = 0 空闲状态，处于这个状态什么都不用做
        /// PlayerOpt = 1 等待玩家操作， 显示倒计时，
        /// GameOver =2, 结束状态， 等待下一局开始
        /// </summary>
        public GameStatus Status { get; set; }

        public List<PlayerInfo> Players { get; set; }

        /// <summary>
        /// 当前奖池
        /// </summary>
        public List<long> Pools { get; set; }

        /// <summary>
        /// 当前状态剩余多少秒
        /// </summary>
        public int TimeLeftMs { get; set; }

        /// <summary>
        /// 玩家操作等待总时长
        /// </summary>
        public int PlayerOptMs { get; set; }

        /// <summary>
        /// 牌局结算总时长
        /// </summary>
        public int GameOverMs { get; set; }
    }
}
