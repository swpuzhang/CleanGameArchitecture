using Account.Domain.Entitys;
using Commons.Db.Mongodb;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Account.Infrastruct.Repository
{
    public class AccountInfoRepository : MongoUserRepository<AccountInfo>, IAccountInfoRepository
    {
        public AccountInfoRepository(IAccountContext context) : base(context.AccountInfos)
        {

        }
        public async Task<AccountInfo> GetByPlatform(string platform)
        {
            var all = await _dbCol.FindAsync(e => e.PlatformAccount == platform);
            return await all.FirstOrDefaultAsync();
        }

        public Task Update(AccountInfo account)
        {
            BsonDocument bs = new BsonDocument("$set", account.ToBsonDocument<AccountInfo>());
            _dbCol.UpdateOneAsync(e => e.Id == account.Id, bs);
            return Task.CompletedTask;
        }
    }
}
