using System;
using System.Collections.Generic;
using System.Text;
using CommonModels.GameModels;

namespace Game.Domain.Logic
{
    public enum SeatStatus
    {
        None,
        Follow,
        Drop,
        Add,
        AllIn,
    }
    public class GameSeat :IComparable<GameSeat>
    {
        public GameSeat(int seatNum)
        {
            SeatNum = seatNum;
        }

        public int SeatNum { get; private set; }
        public GamePlayer PlayerInfo { get; private set; } = null;
        public GamePlayer InGamePlayerInfo { get; private set; } = null;
        public List<PokerCard> HandCards { get; private set; } = null;

        public long TotalBetedCoins { get; private set; } = 0;

        public long BetedCoins { get; private set; } = 0;

        public SeatStatus Status { get; private set; } = SeatStatus.None;

        public CardCombination Combination { get; private set; } = new CardCombination() ;

        public long WinCoins { get; private set; } = 0;

        public CardCombination GetAppHandCards()
        {
            if (!IsInGame())
            {
                return null;
            }
            if (!IsCanContinue())
            {
                return null;
            }
         
            var combine = new CardCombination(Combination.ComType, HandCards, Combination.Point);
            return combine;
        }

        public void Win(long coins)
        {
            WinCoins = coins;
        }

        public void Standup()
        {
            PlayerInfo = null;
            Clean();
        }

        public bool IsSeated()
        {
            return PlayerInfo != null;
        }
     
        public void PlayerSeat(GamePlayer player)
        {
            PlayerInfo = player;
        }

        public void DealCard(List<PokerCard> cards, long blind)
        {
            Status = SeatStatus.None;
            BetedCoins = 0;
            InGamePlayerInfo = PlayerInfo;
            HandCards = cards;
            TotalBetedCoins = blind;
            WinCoins = 0;
            InGamePlayerInfo.BetCoins(blind);
        }

        public bool IsInGame()
        {
            return InGamePlayerInfo != null;
        }

        public void Clean()
        {
            InGamePlayerInfo = null;

            HandCards = null;

            TotalBetedCoins = 0;

            BetedCoins = 0;

            Status = SeatStatus.None;

            Combination.Reset();

            WinCoins = 0;
        }

        public void SecondRoundStarted(PokerCard card)
        {
            if (!IsInGame())
            {
                return;
            }
            BetedCoins = 0;
            HandCards.Add(card);
            Combination.SetCards(HandCards);
            if (Status != SeatStatus.Drop && Status != SeatStatus.AllIn)
            {
                Status = SeatStatus.None;
            }
        }

        public bool IsCanContinue()
        {
            if (!IsInGame() || Status == SeatStatus.Drop)
            {
                return false;
            }
            return true;
        }

        public bool IsAllin()
        {
            if (IsInGame() && Status == SeatStatus.AllIn)
            {
                return true;
            }
            return false;
        }

        public bool IsCanActive(long maxAdd)
        {
            if (!IsInGame() || Status == SeatStatus.Drop || Status == SeatStatus.AllIn)
            {
                return false;
            }

            if (Status == SeatStatus.Follow || Status == SeatStatus.Add)
            {
                if (BetedCoins < maxAdd)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
        public void Drop()
        {
            
            Status = SeatStatus.Drop;
        }

        public bool Follow(long chips)
        {
            if(!BetCoins(chips))
            {
                return false;
            }
            Status = SeatStatus.Follow;
            if (InGamePlayerInfo.Carry == 0)
            {
                Status = SeatStatus.AllIn;
            }
            return true;
        }

        public bool Add(long chips)
        {
            if (!BetCoins(chips))
            {
                return false;
            }
            
            Status = SeatStatus.Add;
            if (InGamePlayerInfo.Carry == 0)
            {
                Status = SeatStatus.AllIn;
            }
            return true;
        }

        public int CompareTo(GameSeat other)
        {
            if (Combination == null)
            {
                return -1;
            }
            return Combination.CompareTo(other.Combination);
        }

        public bool BetCoins(long coins)
        {
            if (InGamePlayerInfo.Carry < coins)
            {
                return false;
            }
            BetedCoins += coins;
            TotalBetedCoins += coins;
            InGamePlayerInfo.AddCarry(-coins);
            
            return true;
        }
    }
}
