using Commons.Db.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Money.Domain.Entitys;
using Commons.DiIoc;

namespace Money.Infrastruct.Repository
{
    public interface IMoneyRedisRepository : IRedisRepository, IDependency
    {
        
        Task SetMoneyInfo(MoneyInfo info);

        Task<MoneyInfo> GetMoneyInfo(long id);
    }
}
