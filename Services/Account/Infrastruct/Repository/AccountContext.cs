using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Account.Domain.Entitys;
using Commons.Db.Mongodb;
using Commons.DiIoc;
using MongoDB.Driver;

namespace Account.Infrastruct.Repository
{
    public interface IAccountContext : IDependency
    {
        public IMongoCollection<AccountInfo> AccountInfos { get;}
        public IMongoCollection<UserIdGenInfo> GenInfos { get; }
        public IMongoCollection<LevelConfig> LevelConfigs { get; }
        public IMongoCollection<LevelInfo> LevelInfos { get; }
        public IMongoCollection<GameInfo> GameInfos { get; }
    }
    public class AccountContext : MongoContext, IAccountContext
    {
        public IMongoCollection<AccountInfo> AccountInfos { get; }
        public IMongoCollection<UserIdGenInfo> GenInfos { get; }
        public IMongoCollection<LevelConfig> LevelConfigs { get; }
        public IMongoCollection<LevelInfo> LevelInfos { get; }
        public IMongoCollection<GameInfo> GameInfos { get; }
        public AccountContext(IMongoSettings settings) : base(settings)
        {
            AccountInfos = base._database.GetCollection<AccountInfo>(nameof(AccountInfo));
            GenInfos = base._database.GetCollection<UserIdGenInfo>(nameof(UserIdGenInfo));
            LevelConfigs = base._database.GetCollection<LevelConfig>(nameof(LevelConfig));
            LevelInfos = base._database.GetCollection<LevelInfo>(nameof(LevelInfo));
            GameInfos = base._database.GetCollection<GameInfo>(nameof(GameInfo));
        }
    }
}
