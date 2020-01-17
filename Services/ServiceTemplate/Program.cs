using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Commons.Startup;
using Serilog;
using Commons.LogConfig;
using Autofac.Extensions.DependencyInjection;

namespace ServiceTemplate
{
    public class Program
    {
        public static readonly string AppName = typeof(Program).Namespace;
        public static void Main(string[] args)
        {
            var config = ConfigStartup.GetConfiguration(args);
            Log.Logger = LogConfig.CreateSerilogLogger(config, AppName);
            Log.Information("CreateHostBuilder ({ApplicationContext})...", AppName);
            CreateHostBuilder(args, config).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args, IConfiguration configuratioin) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseSerilog();
                    webBuilder.UseConfiguration(configuratioin);
                    webBuilder.UseUrls($"http://{configuratioin["BindIp"]}:{configuratioin["Port"]}");
                });
    }
}
