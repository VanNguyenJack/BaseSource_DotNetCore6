using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BaseSource.Domain
{
    public interface ISpecification<T>
    {
        int Take { get; set; }
        int Skip { get; set; }
        bool IsPagingEnabled { get; set; }

        Expression<Func<T, bool>> Criteria { get; }
        List<Expression<Func<T, object>>> Includes { get; }
        Dictionary<Expression<Func<T, object>>, string> OrderBy { get; }
        Dictionary<Expression<Func<T, object>>, string> ThenBy { get; }
    }
}
