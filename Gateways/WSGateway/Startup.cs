using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Commons.Startup;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using WSGateway.Hubs;
using WSGateway.Mapper;

namespace WSGateway
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
            services.AddSignalR();
            ConfigStartup.ConfigureCommonServices(services, Configuration);
            services.AddMediatR(typeof(Startup));
            ConfigStartup.ConfigAutoMapperServices(services, typeof(MappingProfile));
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            ConfigStartup.ConfigureCommonServices(builder);
            ConfigStartup.ConfigMassTransitSerivces(builder, Configuration, typeof(Startup).Assembly, x =>
            {
               
            });
            builder.RegisterType<HostedService>().As<IHostedService>().SingleInstance();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment _)
        {
            ConfigStartup.ConfigureCommon(app, Configuration);

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<AppHub>("/AppHub");
            });
        }
    }
}
