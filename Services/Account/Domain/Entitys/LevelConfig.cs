using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account.Domain.Entitys
{
    public class LevelConfig
    {
        public LevelConfig()
        {
        }

        [JsonConstructor]
        public LevelConfig(int level, int expNeed)
        {
            Level = level;
            NeedExp = expNeed;
        }

        public int Level { get; private set; }
        public int NeedExp { get; private set; }
    }
}
