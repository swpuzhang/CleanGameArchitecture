using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Commons.Extenssions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;

namespace ApiGateway
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
            services.AddOcelot(
                new ConfigurationBuilder().AddJsonFile(
                    "Ocelot.json", optional: false, reloadOnChange: true)
                .Build())
                .AddConsul();
                //.AddConfigStoredInConsul();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("ApiGateway", new OpenApiInfo { Title = "ApiGateway", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwagger().UseSwaggerUI(options =>
            {
                var apis = Configuration.GetSection("Services").Get<string[]>();
                apis.ForEach(p =>
                {
                    options.SwaggerEndpoint($"/{p}/swagger.json", p);
                });
                options.RoutePrefix = string.Empty;
            });
            app.UseOcelot().Wait();
        }
    }
}
