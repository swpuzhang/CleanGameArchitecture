using System;
using System.Collections.Generic;
using System.Text;

namespace Account.ViewModels
{
    public class GameInfoVm
    {

        public GameInfoVm()
        {
        }


        public GameInfoVm(int gameTimes, int winTimes, long maxWinCoins)
        {
            GameTimes = gameTimes;
            WinTimes = winTimes;
            MaxWinCoins = maxWinCoins;
        }

        public int GameTimes { get; set; }
        public int WinTimes { get; set; }
        public long MaxWinCoins { get; set; }
    }
}
