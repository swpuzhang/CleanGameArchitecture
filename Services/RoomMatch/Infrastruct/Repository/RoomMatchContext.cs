using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RoomMatch.Domain.Entitys;
using Commons.Db.Mongodb;
using Commons.DiIoc;
using MongoDB.Driver;

namespace RoomMatch.Infrastruct.Repository
{
    public interface IRoomMatchContext : IDependency
    {
        public IMongoCollection<CoinsRangeConfig> CoinsRangeConfigs { get; }
        public IMongoCollection<RoomListConfig> RoomListConfigs { get; }
    }
    public class AccountContext : MongoContext, IRoomMatchContext
    {
        public IMongoCollection<CoinsRangeConfig> CoinsRangeConfigs { get; }
        public IMongoCollection<RoomListConfig> RoomListConfigs { get; }
        public AccountContext(IMongoSettings settings) : base(settings)
        {
            CoinsRangeConfigs = base._database.GetCollection<CoinsRangeConfig>(nameof(CoinsRangeConfig));
            RoomListConfigs = base._database.GetCollection<RoomListConfig>(nameof(RoomListConfig));
        }
    }
}
