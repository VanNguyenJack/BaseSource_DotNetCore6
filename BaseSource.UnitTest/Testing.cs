using BaseSource.API;
using BaseSource.Domain.Services;
using BaseSource.Entity.DbContexts;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Respawn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSource.UnitTest
{
    [SetUpFixture]
    public partial class Testing
    {
        private static WebApplicationFactory<Program> _factory = null!;
        private static IConfiguration _configuration = null!;
        private static IServiceScopeFactory _scopeFactory = null!;
        private static Checkpoint _checkpoint = null!;
        private static string? _currentUserId;
        private static ILogger _logger;

        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            _factory = new CustomWebApplicationFactory();
            _scopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory>();
            _configuration = _factory.Services.GetRequiredService<IConfiguration>();

            var loggerFactory = _factory.Services.GetRequiredService<ILoggerFactory>();
            _logger = loggerFactory.CreateLogger("app");

            _checkpoint = new Checkpoint
            {
                TablesToIgnore = new[] { "__EFMigrationsHistory" }
            };
        }

        public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            using var scope = _scopeFactory.CreateScope();

            var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

            return await mediator.Send(request);
        }

        public static string? GetCurrentUserId()
        {
            return _currentUserId;
        }

        public static async Task<string> RunAsDefaultUserAsync()
        {
            return await RunAsUserAsync("ThinDo@local", "UsersPwd", Array.Empty<string>());
        }

        public static async Task<string> RunAsUserAsync(string userNameOrEmail, string password, string[] roles)
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<BaseSourceDbContext>();
            var user = await context.Accounts.AsNoTracking().Where(x => x.Email.ToLower() == userNameOrEmail.ToLower()).FirstOrDefaultAsync();
            if (user.HashedPassword.Equals(password))
            {
                return _currentUserId = user.Uid.ToString();
            }

            return string.Empty;
        }

        public static async Task ResetState()
        {

            var cn = new SqlConnectionStringBuilder(_configuration.GetConnectionString("DefaultConnectionString"));
            cn.ConnectTimeout = 60;
            await _checkpoint.Reset(cn.ToString());

            _currentUserId = null;
        }

        public static async Task<TEntity?> FindAsync<TEntity>(params object[] keyValues)
            where TEntity : class
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<BaseSourceDbContext>();

            return await context.FindAsync<TEntity>(keyValues);
        }

        public static async Task<List<TEntity>> GetAllAsync<TEntity>() where TEntity : class
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<BaseSourceDbContext>();

            return await context.Set<TEntity>().ToListAsync();
        }

        public static async Task AddAsync<TEntity>(TEntity entity)
            where TEntity : class
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<BaseSourceDbContext>();

            context.Add(entity);

            await context.SaveChangesAsync();
        }

        public static async Task<int> CountAsync<TEntity>() where TEntity : class
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<BaseSourceDbContext>();

            return await context.Set<TEntity>().CountAsync();
        }

        public static async Task<string> GetMessageAsync(string alertId, params object[] parameters)
        {
            using var scope = _scopeFactory.CreateScope();

            var alertService = scope.ServiceProvider.GetRequiredService<IGlbAlertService>();
            return await alertService.GetMessageAsync(alertId, parameters);
        }

        public static async Task<string> GetMessageAsync(string alertId)
        {
            using var scope = _scopeFactory.CreateScope();

            var alertService = scope.ServiceProvider.GetRequiredService<IGlbAlertService>();
            return await alertService.GetMessageAsync(alertId);
        }

        [OneTimeTearDown]
        public void RunAfterAnyTests()
        {
        }
    }
}
