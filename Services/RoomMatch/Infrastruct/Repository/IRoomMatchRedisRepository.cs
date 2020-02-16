using Commons.Db.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RoomMatch.Domain.Entitys;
using Commons.DiIoc;

namespace RoomMatch.Infrastruct.Repository
{
    public interface IRoomMatchRedisRepository : IRedisRepository, IDependency
    {
        
        Task SetRoomMatchInfo(RoomMatchInfo info);

        Task<RoomMatchInfo> GetRoomMatchInfo(long id);
    }
}
