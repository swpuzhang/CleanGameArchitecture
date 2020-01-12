using ServiceTemplate.Domain.Entitys;
using Commons.Db.Mongodb;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace ServiceTemplate.Infrastruct.Repository
{
    public class ServiceTemplateInfoRepository : MongoUserRepository<ServiceTemplateInfo>, IServiceTemplateInfoRepository
    {
        public ServiceTemplateInfoRepository(IServiceTemplateContext context) : base(context.ServiceTemplateInfos)
        {

        }
       
    }
}
