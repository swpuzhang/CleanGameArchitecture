using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceTemplate.Domain.Entitys;
using Commons.Db.Mongodb;
using Commons.DiIoc;
using MongoDB.Driver;

namespace ServiceTemplate.Infrastruct.Repository
{
    public interface IServiceTemplateContext : IDependency
    {
        public IMongoCollection<ServiceTemplateInfo> ServiceTemplateInfos { get;}
    }
    public class ServiceTemplateContext : MongoContext, IServiceTemplateContext
    {
        public IMongoCollection<ServiceTemplateInfo> ServiceTemplateInfos { get; }
       
        public ServiceTemplateContext(IMongoSettings settings) : base(settings)
        {
            ServiceTemplateInfos = base._database.GetCollection<ServiceTemplateInfo>(typeof(ServiceTemplateInfo).Name);
        }
    }
}
