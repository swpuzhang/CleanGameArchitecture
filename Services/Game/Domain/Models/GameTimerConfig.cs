using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Game.Domain.Models
{
    public static class GameTimerConfig
    {
        public static int ReadyWait { get; private set; } = 2000;

        public static int DealCard { get; private set; } = 3000;

        public static int DealThirdCard { get; private set; } = 3000;

        public static int BetChips { get; private set; } = 10000;

        public static int GameAccount { get; private set; } = 2000;
    }
}
