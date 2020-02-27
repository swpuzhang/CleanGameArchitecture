using Game.Domain.Entitys;
using Commons.Db.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Commons.Tools.KeyGen;

namespace Game.Infrastruct.Repository
{
    public class GameRedisRepository : RedisRepository, IGameRedisRepository
    {
        public Task<GameInfo> GetGameInfo(long id)
        {
            return RedisOpt.GetStringAsync<GameInfo>(KeyGenTool.GenUserKey(id,
                nameof(GameInfo)));
        }


        public Task SetGameInfo(GameInfo info)
        {
            return RedisOpt.SetStringAsync(KeyGenTool.GenUserKey(info.Id,
               nameof(GameInfo)), info, TimeSpan.FromDays(7));
        }

    }
}
