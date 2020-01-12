using Account.Domain.Entitys;
using Commons.Db.Mongodb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Account.Infrastruct.Repository
{
    public class UserIdGenRepository : MongoUserRepository<UserIdGenInfo>, IUserIdGenRepository
    {
        public UserIdGenRepository(IAccountContext context) : base(context.GenInfos)
        {

        }

        public async Task<long> GenNewId()
        {

            var options = new FindOneAndUpdateOptions<UserIdGenInfo>
            {
                ReturnDocument = ReturnDocument.After
            };

            var update = Builders<UserIdGenInfo>.Update.Inc(nameof(UserIdGenInfo.UserId), 1);

            var ret = await _dbCol.FindOneAndUpdateAsync<UserIdGenInfo>(new BsonDocument(), update, options);
            return ret.UserId;
        }

    }
}
