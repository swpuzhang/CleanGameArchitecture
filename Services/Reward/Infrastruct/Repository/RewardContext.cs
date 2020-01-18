using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Reward.Domain.Entitys;
using Commons.Db.Mongodb;
using Commons.DiIoc;
using MongoDB.Driver;

namespace Reward.Infrastruct.Repository
{
    public interface IRewardContext : IDependency
    {
        public IMongoCollection<RegisterRewardInfo> RegisterRewardInfos { get; }
        public IMongoCollection<GameActivityConfig> GameActivityConfigs { get; }
        public IMongoCollection<RegisterRewardConfig> RegisterRewardConfigs { get; }
        public IMongoCollection<LoginRewardConfig> LoginRewardConfigs { get; }
        public IMongoCollection<BankruptcyConfig> BankruptcyConfigs { get; }
        public IMongoCollection<InviteRewardConfig> InviteRewardConfigs { get; }
    }
    public class RewardContext : MongoContext, IRewardContext
    {
        public IMongoCollection<RegisterRewardInfo> RegisterRewardInfos { get; }
        public IMongoCollection<GameActivityConfig> GameActivityConfigs { get; }

        public IMongoCollection<RegisterRewardConfig> RegisterRewardConfigs { get; }
        public IMongoCollection<LoginRewardConfig> LoginRewardConfigs { get; }
        public IMongoCollection<BankruptcyConfig> BankruptcyConfigs { get; }
        public IMongoCollection<InviteRewardConfig> InviteRewardConfigs { get; }
        public RewardContext(IMongoSettings settings) : base(settings)
        {
            RegisterRewardInfos = base._database.GetCollection<RegisterRewardInfo>(nameof(RegisterRewardInfo));
            GameActivityConfigs = base._database.GetCollection<GameActivityConfig>(nameof(GameActivityConfig));
            RegisterRewardConfigs = base._database.GetCollection<RegisterRewardConfig>(nameof(RegisterRewardConfig));
            LoginRewardConfigs = base._database.GetCollection<LoginRewardConfig>(nameof(LoginRewardConfig));
            BankruptcyConfigs = base._database.GetCollection<BankruptcyConfig>(nameof(BankruptcyConfig));
            InviteRewardConfigs = base._database.GetCollection<InviteRewardConfig>(nameof(InviteRewardConfig));

        }
    }
}
