using Commons.Db.Mongodb;
using Reward.Domain.Entitys;
using Reward.Infrastruct.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reward.Infrastruct.Repository
{
    public class GameActivityConfigRepository :
        MongoConfigRepository<GameActivityConfig>, IGameActivityConfigRepository
    {
        public GameActivityConfigRepository(IRewardContext context) : base(context.GameActivityConfigs)
        {

        }
    }
}
