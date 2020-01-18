using Commons.Db.Mongodb;
using Commons.DiIoc;
using Reward.Domain.Entitys;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Reward.Infrastruct.Repository
{
    public interface IRegisterRewardConfigRepository : 
        IMongoConfigRepository<RegisterRewardConfig>, IDependency
    {
    }

    public interface ILoginRewardConfigRepository :
        IMongoConfigRepository<LoginRewardConfig>, IDependency
    {
    }

    public interface IBankruptcyConfigRepository :
        IMongoConfigRepository<BankruptcyConfig>, IDependency
    {
    }
    public interface IInviteRewardConfigRepository :
        IMongoConfigRepository<InviteRewardConfig>, IDependency
    {
    }
    
}
