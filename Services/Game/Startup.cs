using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Commons.Startup;
using Autofac;
using MediatR;
using Commons.Db.Redis;
using Game.Application.AutoMapper;
using Game.Domain.ProcessCommands;
using Commons.Models;
using Game.ViewModels;
using Game.Domain.ProcessEvents;
using Serilog;
using CommonMessages.MqCmds;
using Commons.AspMiddlewares;
using GameMessages.MqCmds;

namespace Game
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            ConfigStartup.ConfigureCommonServices(services, Configuration);
            services.AddMediatR(typeof(Startup));
            ConfigStartup.ConfigAutoMapperServices(services, typeof(MappingProfile));

        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<HostedService>().As<IHostedService>().SingleInstance();
            ConfigStartup.ConfigDependencyServices(builder);
            ConfigStartup.ConfigMassTransitSerivces(builder, Configuration, typeof(Startup).Assembly, x =>
            {
                var rabbitCfg = Configuration.GetSection("Rabbitmq");
                x.AddRequestClient<GetMoneyMqCmd>(new Uri($"{rabbitCfg["Uri"]}Money"));
                x.AddRequestClient<BuyInMqCmd>(new Uri($"{rabbitCfg["Uri"]}Money"));
                x.AddRequestClient<GetAccountInfoMqCmd>(new Uri($"{rabbitCfg["Uri"]}Account"));
                x.AddRequestClient<UserApplySitMqCmd>(new Uri($"{rabbitCfg["Uri"]}{rabbitCfg["Match"]}"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment _)
        {
            ConfigStartup.ConfigureCommon(app, Configuration);
            app.UseRouting();

            app.UseAuthorization();

            app.UseTokenCheck("/api/Account/Login");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
