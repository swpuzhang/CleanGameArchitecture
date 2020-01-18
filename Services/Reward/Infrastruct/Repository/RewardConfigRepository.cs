using Reward.Domain;
using Reward.Domain.Entitys;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Reward.Infrastruct.Repository;
using Commons.Db.Mongodb;

namespace Reward.Infrastruct
{
    public class RegisterRewardConfigRepository :
        MongoConfigRepository<RegisterRewardConfig>, IRegisterRewardConfigRepository
    {
        public RegisterRewardConfigRepository(IRewardContext context) : base(context.RegisterRewardConfigs)
        {

        }
    }

    public class LoginRewardConfigRepository : 
        MongoConfigRepository<LoginRewardConfig>, ILoginRewardConfigRepository
    {

        public LoginRewardConfigRepository(IRewardContext context) : base(context.LoginRewardConfigs)
        {

        }

    }

    public class BankruptcyConfigRepository : 
        MongoConfigRepository<BankruptcyConfig>, IBankruptcyConfigRepository
    {

        public BankruptcyConfigRepository(IRewardContext context) : base(context.BankruptcyConfigs)
        {
        }

    }

    public class InviteRewardConfigRepository : MongoConfigRepository<InviteRewardConfig>, IInviteRewardConfigRepository
    {

        public InviteRewardConfigRepository(IRewardContext context) : base(context.InviteRewardConfigs)
        {
        }
    }

   
}
