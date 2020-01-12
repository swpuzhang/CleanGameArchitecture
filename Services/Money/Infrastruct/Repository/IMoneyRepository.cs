using Money.Domain.Entitys;
using Commons.Db.Mongodb;
using Commons.DiIoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Money.Infrastruct.Repository
{
    public interface IMoneyInfoRepository : IMongoUserRepository<MoneyInfo>, IDependency
    {
       
    }
}
