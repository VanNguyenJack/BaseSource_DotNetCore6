


using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using BaseSource.Domain;
using BaseSource.Domain.Extensions;
using BaseSource.Domain.Repositories;
using System.Collections;
using BaseSource.Entity.DbContexts;

namespace BaseSource.Entity.Repositoties
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BaseSourceDbContext _dbContext;
        private bool _disposed;
        private IDbContextTransaction _transaction;
        private Hashtable _repositories;
        private readonly IConfiguration _configuration;


        public UnitOfWork(BaseSourceDbContext dbContext,
            IConfiguration configuration,
            IAccountRepository accountRepository,
            IVehicleRepository vehicleRepository
        )
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _transaction = _dbContext.Database.BeginTransaction();
            _configuration = configuration;
            Accounts = accountRepository;
            Vehicles = vehicleRepository;
        }

        public async Task<int> Commit(CancellationToken cancellationToken)
        {
            int rowEffected;
            try
            {
                rowEffected = await _dbContext.SaveChangesAsync(cancellationToken);
                if (_transaction != null)
                {
                    await _transaction.CommitAsync(cancellationToken);

                    _dbContext.ChangeTracker.Clear();
                    //Begin new transaction for next commit
                    _transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
                }
            }
            catch
            {
                if (_transaction != null)
                    await _transaction.RollbackAsync(cancellationToken);

                _dbContext.ChangeTracker.Clear();
                //Begin new transaction for next commit
                _transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
                throw;
            }
            return rowEffected;

        }

        public async Task CommitBulk(CancellationToken cancellationToken)
        {
            try
            {
                if (!_configuration["Z.EntityFramework.Extensions:LicenseKey"].IsEmpty())
                {
                    await _dbContext.BulkSaveChangesAsync(cancellationToken);
                }
                else
                {
                    await _dbContext.SaveChangesAsync(cancellationToken);
                }

                if (_transaction != null)
                {
                    await _transaction.CommitAsync(cancellationToken);

                    _dbContext.ChangeTracker.Clear();
                    //Begin new transaction for next commit
                    _transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
                }
            }
            catch (Exception ex)
            {
                if (_transaction != null)
                    await _transaction.RollbackAsync(cancellationToken);

                _dbContext.ChangeTracker.Clear();
                //Begin new transaction for next commit
                _transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
                throw;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _transaction.Dispose();
                    _dbContext.Dispose();
                }
            }
            //dispose unmanaged resources
            _disposed = true;
        }

        public async Task Rollback(CancellationToken cancellationToken)
        {
            if (_transaction != null)
                await _transaction.RollbackAsync(cancellationToken);

            _dbContext.ChangeTracker.Clear();
            //Begin new transaction for next commit
            _transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task<int> TemporarySaveChanges(CancellationToken cancellationToken)
        {
            int rowEffected;
            try
            {
                rowEffected = await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch
            {
                throw;
            }

            return rowEffected;
        }

        public async Task TemporaryBulkSaveChanges(CancellationToken cancellationToken)
        {
            try
            {
                if (!_configuration["Z.EntityFramework.Extensions:LicenseKey"].IsEmpty())
                {
                    await _dbContext.BulkSaveChangesAsync(cancellationToken);
                }
                else
                {
                    await _dbContext.SaveChangesAsync(cancellationToken);
                }
            }
            catch
            {
                throw;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            if (_repositories == null)
                _repositories = new Hashtable();

            var type = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(GenericRepository<>);

                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _dbContext);

                _repositories.Add(type, repositoryInstance);
            }

            return (IGenericRepository<TEntity>)_repositories[type];
        }

        public DateTime GetServerTime()
        {
            var serverDateTime = DateTime.Now;
          
            return serverDateTime;
        }

        #region Repository
        public IAccountRepository Accounts { get; }
        public IVehicleRepository Vehicles { get; }
        #endregion
    }
}
