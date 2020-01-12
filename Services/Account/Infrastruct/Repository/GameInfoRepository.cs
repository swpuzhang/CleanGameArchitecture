using Account.Domain.Entitys;
using Commons.Db.Mongodb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account.Infrastruct.Repository
{
    public class GameInfoRepository : MongoUserRepository<GameInfo>, IGameInfoRepository
    {
        public GameInfoRepository(AccountContext context) : base(context.GameInfos)
        {

        }
    }
}
