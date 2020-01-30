using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace Commons.Startup
{
    
    public class ConsulOptions
    {
        public string HttpEndPoint { get; set; }
    }
    public static class ConsulExtenssion
    { 
       public static IServiceCollection AddConsul(this IServiceCollection services, 
           IConfiguration configuration)
        {
            services.Configure<ServiceOptions>(configuration.GetSection("Service"));
            services.AddSingleton<IConsulClient>(sp => new ConsulClient(config =>
            {
                var consulOptions = sp.GetRequiredService<IOptions<ServiceOptions>>().Value;
                config.Address = new Uri(consulOptions.Consul.HttpEndPoint);
            }));
            return services;
        }

        public static IApplicationBuilder UseConsul(this IApplicationBuilder app,
        IConfiguration configuration)
        {
            IConsulClient consul = app.ApplicationServices.GetRequiredService<IConsulClient>();
            IHostApplicationLifetime appLife = app.ApplicationServices
                .GetRequiredService<IHostApplicationLifetime>();
            IOptions<ServiceOptions> serviceOptions = app.ApplicationServices
                .GetRequiredService<IOptions<ServiceOptions>>();
            var addressIpv4Hosts = NetworkInterface.GetAllNetworkInterfaces()
                //.OrderByDescending(c => c.Speed) linux不支持
                .Where(c => c.NetworkInterfaceType != NetworkInterfaceType.Loopback
                       && c.OperationalStatus == OperationalStatus.Up);
            foreach (var item in addressIpv4Hosts)
            {
                var props = item.GetIPProperties();
                var firstIpV4Address = props.UnicastAddresses
                    .Where(c => c.Address.AddressFamily == AddressFamily.InterNetwork)
                    .Select(c => c.Address)
                    .FirstOrDefault().ToString();
                var serviceId = $"{serviceOptions.Value.ServiceName}_{serviceOptions.Value.ServiceIndex}";

                var httpCheck = new AgentServiceCheck()
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(10),
                    Interval = TimeSpan.FromSeconds(5),
                    HTTP = $"{Uri.UriSchemeHttp}://{firstIpV4Address}:{configuration["Port"]}/ConsulHealthCheck",
                };

                var registration = new AgentServiceRegistration()
                {
                    Checks = new[] { httpCheck },
                    Address = firstIpV4Address.ToString(),
                    ID = serviceId,
                    Name = serviceOptions.Value.ServiceName,
                    Port = Int32.Parse(configuration["Port"])
                };
                consul.Agent.ServiceRegister(registration).GetAwaiter().GetResult();
                appLife.ApplicationStopping.Register(() =>
                {
                    consul.Agent.ServiceDeregister(serviceId).GetAwaiter().GetResult();
                });
            }
            app.Map("/ConsulHealthCheck", s =>
            {
                s.Run(async context =>
                {
                    await context.Response.WriteAsync("ok");
                });
            });
            return app;
        }
    }

}
