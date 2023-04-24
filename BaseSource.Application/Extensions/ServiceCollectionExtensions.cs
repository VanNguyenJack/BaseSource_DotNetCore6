using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using BaseSource.Application.Events;
using BaseSource.Application.Mappings;
using BaseSource.Application.PipelineBahaviors;
using BaseSource.Application.Services.Authentication;
using BaseSource.Application.Services.GlbAlert;
using BaseSource.Application.Settings;
using BaseSource.Application.Utilities;
using BaseSource.Application.Wrappers;
using BaseSource.Domain;
using BaseSource.Domain.Contexts;
using BaseSource.Domain.Repositories;
using BaseSource.Domain.Services;
using BaseSource.Entity.DbContexts;
using BaseSource.Entity.Repositoties;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace BaseSource.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureMapster();
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>));

            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<IGlbAlertService, GlbAlertService>();
            return services;
        }
        public static void AddPersistenceContexts(this IServiceCollection services)
        {
            services.AddScoped<IApplicationDbContext, BaseSourceDbContext>();
        }

        public static IServiceCollection AddEDILayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureMapster();

            return services;
        }

        public static void AddEvent(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IEventPublisher, MediatorPublisher>();
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IVehicleRepository, VehicleRepository>();
        }

        public static void AddContextInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var dbConnection = CryptographyUtility.Decrypt(configuration.GetConnectionString("DefaultConnectionString"));

            services.AddDbContext<BaseSourceDbContext>(opt => opt.UseSqlServer(dbConnection)
                                                              .LogTo(s => System.Diagnostics.Debug.WriteLine(s))
                                                              .EnableDetailedErrors(true)
                                                              .EnableSensitiveDataLogging(true));
            services.AddSingleton(configuration);
            services.Configure<JwtSettings>(configuration.GetSection("JWTSettings"));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })

           .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,//draft comment out when develop
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = configuration["JWTSettings:Issuer"],
                    ValidAudience = configuration["JWTSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSettings:Key"]))
                };
                o.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.ContentType = "application/json";
                            context.Response.Headers.Add("Token-Expired", "True");
                            context.Response.StatusCode = 200;
                            var item = new Message("200", "Token expired", MessageType.Error);
                            var returnResult = JsonConvert.SerializeObject(new { message = new List<Message>() { item } });
                            return context.Response.WriteAsync(returnResult);
                        }
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        var message = new Message("401", context.Exception.Message, MessageType.Error);
                        var result = JsonConvert.SerializeObject(new { message = new List<Message>() { message } });
                        return context.Response.WriteAsync(result);
                    },
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        if (context.Response.Headers.Any())
                        {
                            return context.Response.WriteAsync("");
                        }
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        var message = new Message("401", "Unauthorize", MessageType.Error);
                        var result = JsonConvert.SerializeObject(new { message = new List<Message>() { message } });
                        return context.Response.WriteAsync(result);
                    },
                    OnForbidden = context =>
                    {
                        context.Response.StatusCode = 403;
                        context.Response.ContentType = "application/json";
                        var message = new Message("403", "403 Not have permission", MessageType.Error);
                        var result = JsonConvert.SerializeObject(new { message = new List<Message>() { message } });
                        return context.Response.WriteAsync(result);
                    }
                };
            });
        }
    }
}
