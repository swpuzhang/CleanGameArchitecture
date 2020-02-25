using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Commons.LogConfig;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.DotNet.PlatformAbstractions;
using Serilog;
using Commons.Threading;

namespace WSGateway
{
    public class Program
    {
        public static readonly string AppName = typeof(Program).Namespace;

        public static void Main(string[] args)
        {
            var config = GetConfiguration(args);
            Log.Logger = LogConfig.CreateSerilogLogger(config, AppName);

            Log.Information("CreateWebHostBuilder ({ApplicationContext})...", "Account");
            CreateWebHostBuilder(args, config).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args, IConfiguration configuratioin) =>
            WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(configuratioin)
                .UseSerilog()
                .UseStartup<Startup>();

        private static IConfiguration GetConfiguration(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(ApplicationEnvironment.ApplicationBasePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args);
            return builder.Build();
        }
    }
}
