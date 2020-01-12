using Account.Domain.Entitys;
using Commons.Db.Mongodb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account.Infrastruct.Repository
{
    public class LevelConfigRepository : MongoConfigRepository<LevelConfig>, ILevelConfigRepository
    {
        public LevelConfigRepository(AccountContext context) : base(context.LevelConfigs)
        {

        }
    }
}
