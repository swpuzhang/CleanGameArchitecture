using Commons.Db.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceTemplate.Domain.Entitys;
using Commons.DiIoc;

namespace ServiceTemplate.Infrastruct.Repository
{
    public interface IServiceTemplateRedisRepository : IRedisRepository, IDependency
    {
        
        Task SetServiceTemplateInfo(ServiceTemplateInfo info);

        Task<ServiceTemplateInfo> GetServiceTemplateInfo(long id);
    }
}
