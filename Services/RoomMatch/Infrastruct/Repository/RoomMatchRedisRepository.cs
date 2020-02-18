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
        public Task<UserRoomInfo> GetUserRoomInfo(long id)
        {
            return RedisOpt.GetStringAsync<UserRoomInfo>(KeyGenTool.GenUserKey(id, nameof(UserRoomInfo)));
        }

        public Task SetUserRoomInfo(UserRoomInfo info)
        {
            return RedisOpt.SetStringAsync<UserRoomInfo>(KeyGenTool.GenUserKey(info.Id, nameof(UserRoomInfo)),
                info, TimeSpan.FromHours(1));
        }

        public Task DeleteUserRoomInfo(long id)
        {
            return RedisOpt.DeleteKeyAsync(KeyGenTool.GenUserKey(id, nameof(UserRoomInfo)));
        }
    }
}
