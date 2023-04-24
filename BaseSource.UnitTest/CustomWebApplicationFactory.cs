using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;

using BaseSource.API;
using BaseSource.Application.Services;
using BaseSource.Application.Settings;
using BaseSource.Application.Utilities;
using BaseSource.Domain.Shared;
using BaseSource.Entity.DbContexts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace BaseSource.UnitTest
{
    internal class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        static void SetRequestUrl(HttpRequest httpRequest, string url)
        {
            UriHelper
              .FromAbsolute(url, out var scheme, out var host, out var path, out var query,
                fragment: out var _);

            httpRequest.Scheme = scheme;
            httpRequest.Host = host;
            httpRequest.Path = path;
            httpRequest.QueryString = query;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(configurationBuilder =>
            {
                var integrationConfig = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .AddEnvironmentVariables()
                .Build();
                configurationBuilder.AddConfiguration(integrationConfig);
            });

            builder.ConfigureServices((builder, services) =>
            {
                var dbConnection = CryptographyUtility.Decrypt(builder.Configuration.GetConnectionString("DefaultConnectionString"));
                services
                    .Remove<DbContextOptions<BaseSourceDbContext>>()
                    .AddDbContext<BaseSourceDbContext>((sp, options) =>
                        options.UseSqlServer(dbConnection,
                            builder => builder.MigrationsAssembly(typeof(BaseSourceDbContext).Assembly.FullName)));

                // AddSharedInfrastructure
                services.AddScoped<IDateTimeService, DateTimeService>();

                var currentURL = "https://localhost:44331";
                var httpContext = new DefaultHttpContext();
                httpContext.Request.Headers["X-Custom-Header"] = "88-4-5-11";
                SetRequestUrl(httpContext.Request, currentURL);

                services.AddSingleton<IUriService>(o =>
                {
                    var request = httpContext.Request;
                    var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
                    return new UriService(uri);
                });

            });
        }
    }
}
