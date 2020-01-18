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
using Account.Application.AutoMapper;
using Account.Domain.ProcessCommands;
using Commons.Models;
using Account.ViewModels;
using Account.Domain.ProcessEvents;
using Serilog;
using CommonMessages.MqCmds;
using Commons.AspMiddlewares;

namespace Account
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
            ConfigStartup.ConfigureCommonServices(builder);
            ConfigStartup.ConfigMassTransitSerivces(builder, Configuration, typeof(Startup).Assembly, x =>
            {
                var rabbitCfg = Configuration.GetSection("Rabbitmq");
                Log.Information($"money uri:{rabbitCfg["Uri"]}Money");
                x.AddRequestClient<GetMoneyMqCmd>(new Uri($"{rabbitCfg["Uri"]}Money"));
                x.AddRequestClient<AddMoneyMqCmd>(new Uri($"{rabbitCfg["Uri"]}Money"));
            });
            builder.RegisterType<HostedService>().As<IHostedService>().SingleInstance();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment _)
        {
            
            ConfigStartup.ConfigureCommon(app, Configuration);

            app.UseTokenCheck("/api/Account/Login");

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
           
        }
    }
}
