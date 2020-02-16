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
        public IMongoCollection<RoomMatchInfo> RoomMatchInfos { get;}
    }
    public class RoomMatchContext : MongoContext, IRoomMatchContext
    {
        public IMongoCollection<RoomMatchInfo> RoomMatchInfos { get; }
       
        public RoomMatchContext(IMongoSettings settings) : base(settings)
        {
            RoomMatchInfos = base._database.GetCollection<RoomMatchInfo>(typeof(RoomMatchInfo).Name);
        }
    }
}
