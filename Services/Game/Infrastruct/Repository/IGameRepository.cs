using Game.Domain.Entitys;
using Commons.Db.Mongodb;
using Commons.DiIoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Game.Infrastruct.Repository
{
    public interface IGameInfoRepository : IMongoUserRepository<GameInfo>, IDependency
    {
       
    }
}
