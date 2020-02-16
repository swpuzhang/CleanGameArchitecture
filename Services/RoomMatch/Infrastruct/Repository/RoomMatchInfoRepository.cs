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
    public class RoomMatchInfoRepository : MongoUserRepository<RoomMatchInfo>, IRoomMatchInfoRepository
    {
        public RoomMatchInfoRepository(IRoomMatchContext context) : base(context.RoomMatchInfos)
        {

        }
       
    }
}
