using Reward.Domain.Entitys;
using Commons.Db.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Commons.Db.Mongodb;
using Commons.DiIoc;

namespace Reward.Infrastruct.Repository
{
    public interface IGameActivityConfigRepository : IMongoConfigRepository<GameActivityConfig>, IDependency
    {
    }
}
