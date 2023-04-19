using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSource.Domain.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> Entities { get; }

        IQueryable<T> AsNoTracking();

        Task<T> GetByIdAsync(string id);

        Task<T> GetByIdAsync(int id);

        Task<T> GetAsNoTrackingByIdAsync(string id);

        Task<List<T>> GetAllAsync();

        Task<List<T>> GetPagedResponseAsync(int pageNumber, int pageSize);

        Task<T> AddAsync(T entity);

        Task AddRangeAsync(IEnumerable<T> entities);

        Task UpdateAsync(T entity);

        Task UpdateRangeAsync(IEnumerable<T> entity);

        Task DeleteAsync(T entity);

        Task RemoveRange(IEnumerable<T> entities);

        int Count();

        IEnumerable<T> FindWithSpecificationPattern(ISpecification<T> specification = null);

        IEnumerable<T> FindWithSpecificationPattern(IQueryable<T> queryable, ISpecification<T> specification = null);

        int CountWithSpecificationPattern(ISpecification<T> specification = null);
        Task<int> CountWithSpecificationPatternAsync(ISpecification<T> specification = null);

        int CountWithSpecificationPattern(IQueryable<T> queryable, ISpecification<T> specification = null);
        Task<int> CountWithSpecificationPatternAsync(IQueryable<T> queryable, ISpecification<T> specification = null);

        Task AttachAsync(T entity);

        void DetachEntity(T entity);

        bool Exists(Func<T, bool> predicate);

        TResult GetFieldValue<TResult>(Func<T, bool> predicate, Func<T, TResult> selector);

        List<TResult> GetFieldValueAsList<TResult>(Func<T, bool> predicate, Func<T, TResult> selector);

        Task<IEnumerable<TResult>> ExecuteSqlQuery<TResult>(string sql, object param = null, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = null);

        Task<int> ExecuteAsync(string sql, object parameters = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType commandType = CommandType.StoredProcedure);
        Task<int> ExecuteWithTransactionAsync(string sql, object parameters = null, int? commandTimeout = null, CommandType commandType = CommandType.StoredProcedure);
    }
}
