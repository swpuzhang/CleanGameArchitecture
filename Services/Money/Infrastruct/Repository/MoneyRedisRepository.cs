using Money.Domain.Entitys;
using Commons.Db.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Commons.Tools.KeyGen;

namespace Money.Infrastruct.Repository
{
    public class MoneyRedisRepository : RedisRepository, IMoneyRedisRepository
    {
        public Task<MoneyInfo> GetMoneyInfo(long id)
        {
            return RedisOpt.GetStringAsync<MoneyInfo>(KeyGenTool.GenUserKey(id,
                nameof(MoneyInfo)));
        }


        public Task SetMoneyInfo(MoneyInfo info)
        {
            return RedisOpt.SetStringAsync(KeyGenTool.GenUserKey(info.Id,
               nameof(MoneyInfo)), info, TimeSpan.FromDays(7));
        }

    }
}
