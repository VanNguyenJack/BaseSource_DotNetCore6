using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BaseSource.Domain
{
    public interface IGroupBySpecification<T, TResponse>
    {
        int Take { get; set; }
        int Skip { get; set; }
        bool IsPagingEnabled { get; set; }

        Expression<Func<T, bool>> Criteria { get; }
        List<Expression<Func<T, object>>> Includes { get; }
        Dictionary<Expression<Func<TResponse, object>>, string> OrderBy { get; }
        Dictionary<Expression<Func<TResponse, object>>, string> ThenBy { get; }
        Expression<Func<T, string>> GroupBy { get; }
        Expression<Func<string, IEnumerable<T>, TResponse>> GroupByResultHandler { get; }
    }
}
