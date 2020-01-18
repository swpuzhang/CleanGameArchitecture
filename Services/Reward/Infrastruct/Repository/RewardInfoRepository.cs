using Reward.Domain.Entitys;
using Commons.Db.Mongodb;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Reward.Infrastruct.Repository
{
    public class RegisterRewardRepository : MongoUserRepository<RegisterRewardInfo>, IRegisterRewardRepository
    {
        public RegisterRewardRepository(IRewardContext context) : base(context.RegisterRewardInfos)
        {

        }
       
    }
}
