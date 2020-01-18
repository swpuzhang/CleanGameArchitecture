using Commons.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reward.Domain.Entitys
{
    public class GameSubActInfo : UserEntity
    {

        [JsonConstructor]
        public GameSubActInfo(long curCount, int state)
        {
            CurCount = curCount;
            State = state;
        }

        public long CurCount { get; set; }
        public int State { get; set; }
    }

    public class OneGameActivityInfo : UserEntity
    {
        public OneGameActivityInfo(string activityId, Dictionary<string, GameSubActInfo> countProgress)
        {
            ActivityId = activityId;
            CountProgress = countProgress;
        }

        public string ActivityId { get; private set; }
        public Dictionary<string, GameSubActInfo> CountProgress { get; private set; }
    }
}
