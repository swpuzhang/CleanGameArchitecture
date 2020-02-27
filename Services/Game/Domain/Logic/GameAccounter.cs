using CommonModels.GameModels;
using GameMessages.RoomMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Domain.Logic
{
    public static class GameAccounter
    {
        public static GameOverEvent Caculate(List<GameSeat> seates, List<KeyValuePair<long, List<int>>> poolsSeats, out int winSeat)
        {
            List<KeyValuePair<int, CardCombination>> seatRand = new List<KeyValuePair<int, CardCombination>>();
            foreach(var seat in seates)
            {
                if (seat.IsInGame())
                {
                    seatRand.Add(new KeyValuePair<int, CardCombination>(seat.SeatNum, seat.Combination));
                }
               
            }
            seatRand.Sort((x, y) =>
            {
                if (x.Value == null)
                {
                    return -1;
                }
                return x.Value.CompareTo(y.Value);
             });
            seatRand.Reverse();
            winSeat = seatRand.First().Key;
            List<WinnerCoinsPool> poolWinners = new List<WinnerCoinsPool>();
            foreach (var onePool in poolsSeats)
            {
                int winseat = GetPoolWinner(seatRand, onePool.Value);
                poolWinners.Add(new WinnerCoinsPool(winseat, onePool.Key));
            }
            List<int> winners = GetWinners(seates, poolWinners);
            var playerCards = GetHandCards(seates);
            GameOverEvent gameOver = new GameOverEvent(winners, poolWinners, playerCards);
            return gameOver;
        }

        public static int GetPoolWinner(List<KeyValuePair<int, CardCombination>> seates, List<int> joinedSeats)
        {
            foreach(var oneSeat in seates)
            {
                if (joinedSeats.Contains(oneSeat.Key))
                {
                    return oneSeat.Key;
                }
            }
            return joinedSeats.First();
        }

        public static List<int> GetWinners(List<GameSeat> seates, List<WinnerCoinsPool> pollWinners)
        {
            List<int> winners = new List<int>();
            foreach (var oneSeat in seates)
            {
                long totalWin = pollWinners.Where(x => x.WinnerSeat == oneSeat.SeatNum).Sum(x => x.Coins);
                long profit = totalWin - oneSeat.TotalBetedCoins;
                oneSeat.Win(profit);
                if (profit > 0)
                {
                    winners.Add(oneSeat.SeatNum);
                }

            }
            return winners;
        }

        public static List<PlayerGameOverInfo> GetHandCards(List<GameSeat> seates)
        {
            List<PlayerGameOverInfo> allPlayerCards = new List<PlayerGameOverInfo>();
            foreach (var seat in seates)
            {
                if (seat.IsCanContinue())
                {
                    var playerCards = new PlayerGameOverInfo(seat.InGamePlayerInfo.Id, 
                        seat.SeatNum,seat.WinCoins,
                        seat.InGamePlayerInfo.Carry,
                        seat.HandCards, 
                        (int)seat.Combination.ComType, 
                        seat.Combination.Point);
                    allPlayerCards.Add(playerCards);
                }
            }
            return allPlayerCards;
        }
    }
}
