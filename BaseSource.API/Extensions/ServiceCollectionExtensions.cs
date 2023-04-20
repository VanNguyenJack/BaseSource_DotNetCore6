using Askmethat.Aspnet.JsonLocalizer.Extensions;
using Askmethat.Aspnet.JsonLocalizer.JsonOptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.OpenApi.Models;
using BaseSource.API.Services;
using BaseSource.Application.Localization;
using BaseSource.Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using JsonStringLocalizerFactory = Askmethat.Aspnet.JsonLocalizer.Localizer.JsonStringLocalizerFactory;
using System;
using System.Globalization;
using System.Linq;
using BaseSource.Application.Swagger;
using BaseSource.Domain.Shared;
using BaseSource.Application.Services;

namespace BaseSource.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddSharedInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IUriService>(o =>
            {
                var accessor = o.GetRequiredService<IHttpContextAccessor>();
                var request = accessor.HttpContext.Request;
                var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
                return new UriService(uri);
            });
        }
        internal static IServiceCollection AddCurrentUserService(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<ISessionService, SessionService>();
            return services;
        }

        public static void ConfigurationLocalization(this IServiceCollection services, IConfiguration configuration)
        {
            var jsonLocalizationOptions = configuration.GetSection("LocalizationOptions").Get<JsonLocalizationOptions>();
            var supportedCultures = LocalizationConstants.SupportedLanguages.Select(l => new CultureInfo(l.Code)).ToHashSet();
            services.AddJsonLocalization(options =>
            {
                options.ResourcesPath = jsonLocalizationOptions.ResourcesPath;
                options.CacheDuration = jsonLocalizationOptions.CacheDuration;
                options.FileEncoding = jsonLocalizationOptions.FileEncoding;
                options.UseBaseName = jsonLocalizationOptions.UseBaseName;
                options.IsAbsolutePath = jsonLocalizationOptions.IsAbsolutePath;
                options.SupportedCultureInfos = supportedCultures;
                options.LocalizationMode = LocalizationMode.Basic;
            });
            services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();
            services.AddSingleton<IStringLocalizer, JsonStringLocalizer>();
        }

        public static void AddEssentials(this IServiceCollection services)
        {
            services.RegisterSwagger();
            services.AddVersioning();
        }

        private static void RegisterSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Base Source Web API"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    BearerFormat = "JWT",
                    Description = "Input your Bearer token in this format: 'Bearer {token}' to access this API"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            },
                              Scheme = "oauth2",
                              Name = "Bearer",
                              In = ParameterLocation.Header
                        },
                        Array.Empty<string>()
                    }
                });
                c.OperationFilter<SwaggerOperationNameFilter>();
            });

        }

        private static void AddVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(config =>
            {
                // Specify the default API Version as 1.0
                config.DefaultApiVersion = new ApiVersion(1, 0);
                // If the client hasn't specified the API version in the request, use the default API version number 
                config.AssumeDefaultVersionWhenUnspecified = true;
                // Advertise the API versions supported for the particular endpoint
                config.ReportApiVersions = true;
            });
        }
    }
}
