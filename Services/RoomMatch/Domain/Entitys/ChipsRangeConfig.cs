using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoomMatch.Domain.Entitys
{
    public class CoinsRangeConfig
    {
        public CoinsRangeConfig()
        {
        }

        [JsonConstructor]
        public CoinsRangeConfig(long coinsBegin, long coinsEnd, long blind)
        {
            CoinsBegin = coinsBegin;
            CoinsEnd = coinsEnd;
            Blind = blind;
        }
 
       
        public long CoinsBegin { get; set; }
        /// <summary>
        /// 不包含ChipsEnd
        /// </summary>
        public long CoinsEnd { get; set; }

        public long Blind { get; set; }
    }
}
