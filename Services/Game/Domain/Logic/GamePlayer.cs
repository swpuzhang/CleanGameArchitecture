using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Domain.Logic
{
    public class GamePlayer
    {
        public GamePlayer(long id, string platformAccount,
            string userName, int sex, string headUrl,
            long coins, long diamonds, long carry)
        {
            Id = id;
            PlatformAccount = platformAccount;
            UserName = userName;
            Sex = sex;
            HeadUrl = headUrl;
            Coins = coins;
            Diamonds = diamonds;
            Carry = carry;
        }

        public GamePlayer(long id, long coins, long diamonds, long carry)
        {
            Id = id;
            PlatformAccount = "";
            UserName = "";
            Sex = 0;
            HeadUrl = "";
            Coins = coins;
            Diamonds = diamonds;
            Carry = carry;
        }

        public bool IsSeated()
        {
            return SeatInfo != null;
        }

        public bool BetCoins(long coins)
        {
            if (Carry < coins)
            {
                return false;
            }
            Carry -= coins;
            return true;
        }

        public void Seat(GameSeat seatInfo)
        {
            SeatInfo = seatInfo;
            SeatInfo.PlayerSeat(this);
        }

        public void Standup()
        {
            SeatInfo.Standup();
            Coins += Carry;
            Carry = 0;
            SeatInfo = null;
        }

        public void BuyIn(long coins, long diamonds, long carry)
        {
            Coins = coins;
            Diamonds = diamonds;
            Carry = carry;
        }

        public void AddCarry(long addCarry)
        {
            Carry += addCarry;
        }

        public void UpdateInfo(long id, string platformAccount,
            string userName, int sex, string headUrl,
            long coins, long diamonds, long carry)
        {
            Id = id;
            PlatformAccount = platformAccount;
            UserName = userName;
            Sex = sex;
            HeadUrl = headUrl;
            Coins = coins;
            Diamonds = diamonds;
            Carry = carry;
        }

        public void FlushAlive()
        {
            LastAliveTime = DateTime.Now;
        }

        public bool IsAlive()
        {
            return DateTime.Now - LastAliveTime < TimeSpan.FromSeconds(60);
        }

        public Int64 Id { get; private set; }
        public GameSeat SeatInfo { get; private set; } = null;
        public string PlatformAccount { get; private set; }
        public string UserName { get; private set; }
        public int Sex { get; set; }
        public string HeadUrl { get; private set; }
        
        public long Coins { get; private set; }

        public long Diamonds { get; private set; }

        public long Carry { get; private set; } = 0;

        public DateTime LastAliveTime { get; private set; } = DateTime.Now;
    }
}
