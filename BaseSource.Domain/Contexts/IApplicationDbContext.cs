using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace BaseSource.Domain.Contexts
{
    public interface IApplicationDbContext : IDisposable
    {
        IDbConnection Connection { get; }
        DatabaseFacade Database { get; }
        bool HasChanges { get; }
        EntityEntry Entry(object entity);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
