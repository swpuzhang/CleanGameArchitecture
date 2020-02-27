using CommonModels.GameModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameMessages.MqCmds
{
    public class GameLogMqCommand
    {
        public GameLogMqCommand(GameLog gameLog)
        {
            GameLog = gameLog;
        }

        public GameLog GameLog { get; set; }

        public Dictionary<long, long> GetPlayers()
        {
            var gameOverAct = GameLog.GameActions.Where(x => x.ActionName == GameLog.gameOver).First() as GameOverAct;
            Dictionary<long, long> allplayers = new Dictionary<long, long>();
            foreach (var one in gameOverAct.Players)
            {
                allplayers.Add(one.Id, one.CoinsInc);
            }
            return allplayers;
        }
    }
}
