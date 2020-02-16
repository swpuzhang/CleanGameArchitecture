using GameMessages.MqCmds;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoomMatch.Domain.Entitys
{
   
    public class RoomListConfig
    {
        public RoomListConfig()
        {
        }

        public RoomListConfig(RoomTypes roomType, long blind, long minCoins, long maxCoins,
            int tipsPersent, long minCarry, long maxCarry)
        {

            Blind = blind;
            MinCoins = minCoins;
            MaxCoins = maxCoins;
            TipsPersent = tipsPersent;
            MinCarry = minCarry;
            MaxCarry = maxCarry;
            RoomType = roomType;
        }

        public RoomTypes RoomType { get; private set; }
        public long Blind { get; private set; }
        public long MinCoins { get; private set; }
        public long MaxCoins { get; private set; }
        public int TipsPersent { get; private set; }
        public long MinCarry { get; private set; }
        public long MaxCarry { get; private set; }
    }
}
