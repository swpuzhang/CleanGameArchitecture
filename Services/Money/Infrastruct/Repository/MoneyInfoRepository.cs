using Money.Domain.Entitys;
using Commons.Db.Mongodb;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Money.Infrastruct.Repository
{
    public class MoneyInfoRepository : MongoUserRepository<MoneyInfo>, IMoneyInfoRepository
    {
        public MoneyInfoRepository(IMoneyContext context) : base(context.MoneyInfos)
        {

        }
       
    }
}
