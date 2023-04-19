using BaseSource.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BaseSource.Entity.Repositoties
{
    public class SpecificationEvaluator<TEntity> where TEntity : class
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> spec)
        {
            var query = inputQuery;

            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }

            query = GetOrderBy(spec, query);

            if (spec.IsPagingEnabled)
            {
                query = query.Skip(spec.Skip)
                    .Take(spec.Take);
            }

            query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

            return query;
        }

        public static IQueryable<TEntity> GetOrderBy(ISpecification<TEntity> spec, IQueryable<TEntity> query)
        {
            if (spec.OrderBy != null)
            {
                foreach (var orderBy in spec.OrderBy)
                {
                    if (orderBy.Value == "asc")
                        query = Queryable.OrderBy((IOrderedQueryable<TEntity>)query, orderBy.Key);
                    else
                        query = Queryable.OrderByDescending((IOrderedQueryable<TEntity>)query, orderBy.Key);
                }
                if (spec.ThenBy != null)
                    foreach (var thenBy in spec.ThenBy)
                    {
                        if (thenBy.Value == "asc")
                            query = Queryable.ThenBy((IOrderedQueryable<TEntity>)query, thenBy.Key);
                        else
                            query = Queryable.ThenByDescending((IOrderedQueryable<TEntity>)query, thenBy.Key);
                    }
            }

            return query;
        }

        public static IQueryable<TEntity> GetTotal(IQueryable<TEntity> inputQuery, ISpecification<TEntity> spec)
        {
            var query = inputQuery;

            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }

            return query;
        }
    }
}
