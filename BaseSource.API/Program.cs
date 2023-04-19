using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Hosting;
using BaseSource.Entity.DbContexts;

namespace BaseSource.API
{
    public class Program
    {
        private static string HostingEnvironment => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        private static bool IsEnvironment(string environmentName) => HostingEnvironment?.ToLower() == environmentName?.ToLower() && null != environmentName;

        private static bool Development => IsEnvironment(Environments.Development);
        private static bool Production => IsEnvironment(Environments.Production);
        private static bool Staging => IsEnvironment(Environments.Staging);
        public static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            string appSettingFile = "appsettings.json";
            if (Production)
            {
                appSettingFile = $"appsettings.{Environments.Production}.json";
            }
            else if (Staging)
            {
                appSettingFile = $"appsettings.{Environments.Staging}.json";
            }
            else if (Development)
            {
                appSettingFile = $"appsettings.{Environments.Development}.json";
            }
            builder.AddJsonFile(appSettingFile, optional: true, reloadOnChange: true);
            var config = builder.Build();
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(config).CreateLogger();
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger("app");
                var context = services.GetRequiredService<BaseSourceDbContext>();
            }
            host.Run();
            Log.CloseAndFlush();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
