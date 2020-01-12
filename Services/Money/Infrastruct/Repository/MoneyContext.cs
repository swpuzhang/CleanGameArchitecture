using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Money.Domain.Entitys;
using Commons.Db.Mongodb;
using Commons.DiIoc;
using MongoDB.Driver;

namespace Money.Infrastruct.Repository
{
    public interface IMoneyContext : IDependency
    {
        public IMongoCollection<MoneyInfo> MoneyInfos { get;}
    }
    public class MoneyContext : MongoContext, IMoneyContext
    {
        public IMongoCollection<MoneyInfo> MoneyInfos { get; }
       
        public MoneyContext(IMongoSettings settings) : base(settings)
        {
            MoneyInfos = base._database.GetCollection<MoneyInfo>(typeof(MoneyInfo).Name);
        }
    }
}
