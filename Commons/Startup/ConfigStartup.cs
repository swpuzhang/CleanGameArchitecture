using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using Commons.Db.Mongodb;
using Commons.Db.Redis;
using Commons.DiIoc;
using Commons.Models;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.DotNet.PlatformAbstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using AutoMapper;
using MassTransit;
using Serilog;
using System.Threading.Tasks;
using System.Threading;
using MassTransit.AutofacIntegration;

namespace Commons.Startup
{

    public class ServiceOptions
    {
        public string ServiceName { get; set; }
        public int ServiceIndex { get; set; }
        public ConsulOptions Consul { get; set; }
    }

    public static class ConfigStartup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public static void ConfigureCommonServices(IServiceCollection services,
            IConfiguration configuration)
        {
            services.ConfigSwagger();
            ConfigMongoServices(services, configuration);
            services.AddConsul(configuration);
        }

        public static void ConfigureCommonServices(ContainerBuilder builder)
        {
            ConfigDependencyServices(builder);
        }

        public static void ConfigureCommon(IApplicationBuilder app, IConfiguration configuration)
        {
            RedisOpt.Start(configuration["redis:ConnectionString"]);
            ConfigureSwagger(app);
            app.UseConsul(configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static void ConfigureSwagger(IApplicationBuilder app)
        {
            app.UseSwaggerService();
        }

        public static IConfiguration GetConfiguration(string[] args)
        {
            string basePath = Directory.GetCurrentDirectory();
            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args);
            return builder.Build();
        }
       
        public static void ConfigDependencyServices(ContainerBuilder builder)
        {
            var basetype = typeof(IDependency);
            var assms = AppDomain.CurrentDomain.GetAssemblies().ToList();
            builder.RegisterAssemblyTypes(assms.ToArray())
                .Where(t => basetype.IsAssignableFrom(t) && t != basetype)
                .AsImplementedInterfaces().InstancePerLifetimeScope();         
        }
       
        public static void ConfigMongoServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoSettings>(
               configuration.GetSection(nameof(MongoSettings)));

            services.AddSingleton<IMongoSettings>(sp =>
                sp.GetRequiredService<IOptions<MongoSettings>>().Value);
        }

        public static void ConfigAutoMapperServices(this IServiceCollection services, Type mapfile)
        {
            services.AddAutoMapper(mapfile);
        }

        public static void ConfigMassTransitSerivces(ContainerBuilder builder,
            IConfiguration Configuration,
            Assembly assm,
            Action<IContainerBuilderConfigurator> clientAdd)
        {
            builder.AddMassTransit(x =>
            {
                var rabbitCfg = Configuration.GetSection("Rabbitmq");
                x.AddConsumers(assm);
                x.AddBus(context => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    Log.Information($"rabbitCfg host:{rabbitCfg["Uri"]}");
                    var host = cfg.Host(new Uri(rabbitCfg["Uri"]), h =>
                    {
                        h.Username(rabbitCfg["UserName"]);
                        h.Password(rabbitCfg["Passwd"]);

                    });

                    cfg.ReceiveEndpoint(rabbitCfg["Queue"], ec =>
                    {

                        ec.ConfigureConsumers(context);
                        //ec.Consumer(typeof(DoSomethingConsumer), c => Activator.CreateInstance(c));
                        //特殊消息
                        //EndpointConvention.Map<DoSomething>(e.InputAddress);
                    });

                    //cfg.ConfigureEndpoints(context);
                    
                }));
                clientAdd(x);
            });        
        }
    }
}
