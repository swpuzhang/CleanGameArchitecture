using ServiceTemplate.Domain.Entitys;
using Commons.Db.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Commons.Tools.KeyGen;

namespace ServiceTemplate.Infrastruct.Repository
{
    public class ServiceTemplateRedisRepository : RedisRepository, IServiceTemplateRedisRepository
    {
        public Task<ServiceTemplateInfo> GetServiceTemplateInfo(long id)
        {
            return RedisOpt.GetStringAsync<ServiceTemplateInfo>(KeyGenTool.GenUserKey(id,
                nameof(ServiceTemplateInfo)));
        }


        public Task SetServiceTemplateInfo(ServiceTemplateInfo info)
        {
            return RedisOpt.SetStringAsync(KeyGenTool.GenUserKey(info.Id,
               nameof(ServiceTemplateInfo)), info, TimeSpan.FromDays(7));
        }

    }
}
