using Commons.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Account.Domain.Entitys
{
    public class GameInfo : UserEntity
    {
        public static string ClassName = "GameInfo";

        public GameInfo()
        {
        }

        [JsonConstructor]
        public GameInfo(long id, int gameTimes, int winTimes, long maxWinCoins)
        {
            Id = id;
            GameTimes = gameTimes;
            WinTimes = winTimes;
            MaxWinCoins = maxWinCoins;
        }

        public int GameTimes { get; private set; }
        public int WinTimes { get; private set; }
        public long MaxWinCoins { get; private set; }
    }
}
