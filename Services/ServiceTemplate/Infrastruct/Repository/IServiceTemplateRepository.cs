using ServiceTemplate.Domain.Entitys;
using Commons.Db.Mongodb;
using Commons.DiIoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceTemplate.Infrastruct.Repository
{
    public interface IServiceTemplateInfoRepository : IMongoUserRepository<ServiceTemplateInfo>, IDependency
    {
       
    }
}
