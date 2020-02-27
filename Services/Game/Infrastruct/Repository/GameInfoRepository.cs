using Game.Domain.Entitys;
using Commons.Db.Mongodb;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Game.Infrastruct.Repository
{
    public class GameInfoRepository : MongoUserRepository<GameInfo>, IGameInfoRepository
    {
        public GameInfoRepository(IGameContext context) : base(context.GameInfos)
        {

        }
       
    }
}
