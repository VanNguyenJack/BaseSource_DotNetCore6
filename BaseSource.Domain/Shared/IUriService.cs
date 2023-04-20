using BaseSource.Domain.CommonFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSource.Domain.Shared
{
    public interface IUriService
    {
        Uri GetPageUri(PaginationFilter filter, string route);
        Uri GetPageUri(PaginationSpecificationFilter filter, string route);
        Uri GetPageUri(PaginationGroupBySpecificationFilter filter, string route);
    }
}
