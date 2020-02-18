using RoomMatch.Domain.Entitys;
using Commons.Db.Mongodb;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace RoomMatch.Infrastruct.Repository
{
    public class CoinsRangeConfigRepository : MongoConfigRepository<CoinsRangeConfig>, ICoinsRangeConfigRepository
    {
        public CoinsRangeConfigRepository(IRoomMatchContext context) : base(context.CoinsRangeConfigs)
        {

        }
    }
    public class RoomListConfigRepository : MongoConfigRepository<RoomListConfig>, IRoomListConfigRepository
    {
        public RoomListConfigRepository(IRoomMatchContext context) : base(context.RoomListConfigs)
        {

        }
    }
}
