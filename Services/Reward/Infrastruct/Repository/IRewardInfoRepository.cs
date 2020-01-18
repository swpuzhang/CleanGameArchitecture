using Commons.Db.Mongodb;
using Commons.DiIoc;
using Reward.Domain.Entitys;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Reward.Infrastruct.Repository
{
    public interface IRegisterRewardRepository : IMongoUserRepository<RegisterRewardInfo>, IDependency
    {
        
    }
}
