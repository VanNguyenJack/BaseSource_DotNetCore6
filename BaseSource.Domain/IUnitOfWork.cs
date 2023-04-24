using BaseSource.Domain.Repositories;
using System;

namespace BaseSource.Domain
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> Repository<T>() where T : class;

        Task<int> TemporarySaveChanges(CancellationToken cancellationToken);

        Task TemporaryBulkSaveChanges(CancellationToken cancellationToken);

        Task<int> Commit(CancellationToken cancellationToken);

        Task CommitBulk(CancellationToken cancellationToken);

        Task Rollback(CancellationToken cancellationToken);

        DateTime GetServerTime();

        #region Repository
        IAccountRepository Accounts { get; }
        IVehicleRepository Vehicles { get; }
        #endregion
    }
}
