using RoomMatch.Domain.Entitys;
using Commons.Db.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Commons.Tools.KeyGen;

namespace RoomMatch.Infrastruct.Repository
{
    public class RoomMatchRedisRepository : RedisRepository, IRoomMatchRedisRepository
    {
        public Task<RoomMatchInfo> GetRoomMatchInfo(long id)
        {
            return RedisOpt.GetStringAsync<RoomMatchInfo>(KeyGenTool.GenUserKey(id,
                nameof(RoomMatchInfo)));
        }


        public Task SetRoomMatchInfo(RoomMatchInfo info)
        {
            return RedisOpt.SetStringAsync(KeyGenTool.GenUserKey(info.Id,
               nameof(RoomMatchInfo)), info, TimeSpan.FromDays(7));
        }

    }
}
