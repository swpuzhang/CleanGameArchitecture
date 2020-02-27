using Commons.Db.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Game.Domain.Entitys;
using Commons.DiIoc;

namespace Game.Infrastruct.Repository
{
    public interface IGameRedisRepository : IRedisRepository, IDependency
    {
        
        Task SetGameInfo(GameInfo info);

        Task<GameInfo> GetGameInfo(long id);
    }
}
