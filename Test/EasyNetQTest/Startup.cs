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
using EasyNetQ;
using EasyNetQ.AutoSubscribe;
using System.Reflection;

namespace EasyNetQTest
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
            services.AddScoped<IService, Service>();
            services.AddSingleton(RabbitHutch.CreateBus
                ("host=localhost;virtualHost=SkyWatch;username=SkyWatch;password=sky_watch_2019_best"));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            var services = app.ApplicationServices.CreateScope().ServiceProvider;

            var lifeTime = services.GetService<Microsoft.Extensions.Hosting.IHostApplicationLifetime>();
            var bus = services.GetService<IBus>();
            lifeTime.ApplicationStarted.Register(() =>
            {
                var subscriber = new AutoSubscriber(bus, "OrderService1");
                subscriber.Subscribe(Assembly.GetExecutingAssembly());
                subscriber.SubscribeAsync(Assembly.GetExecutingAssembly());
            });

            lifeTime.ApplicationStopped.Register(() => { bus.Dispose(); });
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
