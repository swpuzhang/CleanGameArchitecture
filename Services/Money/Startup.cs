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
using Money.Application.AutoMapper;
using Money.Domain.ProcessCommands;
using Commons.Models;
using Money.ViewModels;
using Money.Domain.ProcessEvents;
using Serilog;
using Messages.MqCmds;
using Commons.AspMiddlewares;

namespace Money
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
