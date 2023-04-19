
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using BaseSource.Domain;
using BaseSource.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using BaseSource.Entity.DbContexts;

namespace BaseSource.Entity.Repositoties
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly BaseSourceDbContext DbContext;

        public GenericRepository(BaseSourceDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public IQueryable<T> Entities => DbContext.Set<T>();

        public virtual async Task<T> GetByIdAsync(string id)
        {
            return await DbContext.Set<T>().FindAsync(id);
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await DbContext.Set<T>().FindAsync(id);
        }

        public async Task<T> GetAsNoTrackingByIdAsync(string id)
        {
            var entity = await DbContext.Set<T>().FindAsync(id);
            if (entity != null)
                DbContext.Entry(entity).State = EntityState.Detached;
            return entity;
        }

        public IQueryable<T> AsNoTracking()
        {
            return DbContext.Set<T>().AsNoTracking();
        }

        public int Count()
        {
            return DbContext.Set<T>().Count();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await DbContext.Set<T>().ToListAsync();
        }

        public async Task<List<T>> GetPagedResponseAsync(int pageNumber, int pageSize)
        {
            return await DbContext
                .Set<T>()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            await DbContext.Set<T>().AddAsync(entity);
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await DbContext.Set<T>().AddRangeAsync(entities);
        }

        public async Task UpdateAsync(T entity)
        {
            await Task.FromResult(DbContext.Entry(entity).State = EntityState.Modified);
        }

        public async Task UpdateRangeAsync(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                await Task.FromResult(DbContext.Entry(entity).State = EntityState.Modified);
            }
            DbContext.Set<T>().UpdateRange(entities);
        }


        public async Task DeleteAsync(T entity)
        {
            await Task.FromResult(DbContext.Set<T>().Remove(entity));
        }

        public async Task RemoveRange(IEnumerable<T> entities)
        {
            DbContext.Set<T>().RemoveRange(entities);
        }

        public IEnumerable<T> FindWithSpecificationPattern(ISpecification<T> specification = null)
        {
            return SpecificationEvaluator<T>.GetQuery(DbContext.Set<T>().AsQueryable(), specification);
        }

        public IEnumerable<T> FindWithSpecificationPattern(IQueryable<T> queryable, ISpecification<T> specification = null)
        {
            return SpecificationEvaluator<T>.GetQuery(queryable, specification);
        }

        public int CountWithSpecificationPattern(ISpecification<T> specification = null)
        {
            var result = SpecificationEvaluator<T>.GetTotal(DbContext.Set<T>().AsQueryable(), specification);

            if (result != null) return result.Count();

            return 0;
        }
        public async Task<int> CountWithSpecificationPatternAsync(ISpecification<T> specification = null)
        {
            var result = SpecificationEvaluator<T>.GetTotal(DbContext.Set<T>().AsQueryable(), specification);

            if (result != null) return await result.CountAsync();

            return 0;
        }

        public int CountWithSpecificationPattern(IQueryable<T> queryable, ISpecification<T> specification = null)
        {
            var result = SpecificationEvaluator<T>.GetTotal(queryable, specification);

            if (result != null) return result.Count();

            return 0;
        }

        public async Task<int> CountWithSpecificationPatternAsync(IQueryable<T> queryable, ISpecification<T> specification = null)
        {
            var result = SpecificationEvaluator<T>.GetTotal(queryable, specification);

            if (result != null) return await result.CountAsync();

            return 0;
        }

        public async Task AttachAsync(T entity)
        {
            await Task.FromResult(DbContext.Set<T>().Attach(entity));
        }

        public void DetachEntity(T entity)
        {
            DbContext.Entry(entity).State = EntityState.Detached;
        }

        public bool Exists(Func<T, bool> predicate)
        {
            return DbContext.Set<T>().AsNoTracking().Any(predicate);
        }

        public TResult GetFieldValue<TResult>(Func<T, bool> predicate, Func<T, TResult> selector)
        {
            var result = DbContext.Set<T>().AsNoTracking()
                .Where(predicate)
                .Select(selector)
                .FirstOrDefault();

            if (result == null)
                return default;

            return result;
        }

        public List<TResult> GetFieldValueAsList<TResult>(Func<T, bool> predicate, Func<T, TResult> selector)
        {
            var result = DbContext.Set<T>().AsNoTracking()
                .Where(predicate)
                .Select(selector)
                .ToList();

            if (result == null)
                return default;
            return result;
        }

        public async Task<IEnumerable<TResult>> ExecuteSqlQuery<TResult>(string sql, object parameters = null, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = null)
        {
            var connection = new SqlConnection(DbContext.Database.GetConnectionString());
            using (connection)
            {
                connection.Open();
                var result = await connection.QueryAsync<TResult>(sql, parameters, commandTimeout: commandTimeout, commandType: commandType);
                return result;
            }
        }

        public async Task<int> ExecuteAsync(string sql, object parameters = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType commandType = CommandType.StoredProcedure)
        {
            var connection = new SqlConnection(DbContext.Database.GetConnectionString());
            using (connection)
            {
                connection.Open();
                int result = 0;
                if (commandTimeout != null)
                {
                    result = await connection.ExecuteAsync(sql, parameters, commandTimeout: commandTimeout, commandType: commandType);
                }
                else
                {
                    result = await connection.ExecuteAsync(sql, parameters, commandType: commandType);
                }

                return result;
            }
        }
        public async Task<int> ExecuteWithTransactionAsync(string sql, object parameters = null, int? commandTimeout = null, CommandType commandType = CommandType.StoredProcedure)
        {
            int result = 0;
            using (var transactionScope = new TransactionScope())
            {
                var connection = new SqlConnection(DbContext.Database.GetConnectionString());
                using (connection)
                {
                    connection.Open();
                    try
                    {
                        if (commandTimeout != null)
                        {
                            result = await connection.ExecuteAsync(sql, parameters, commandTimeout: commandTimeout, commandType: commandType);
                        }
                        else
                        {
                            result = await connection.ExecuteAsync(sql, parameters, commandType: commandType);
                        }

                        transactionScope.Complete();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            return result;
        }
    }
}
