using BaseSource.Domain.Constants;
using System;

namespace BaseSource.Domain.CommonFilters
{
    public class PaginationFilter
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 50;

        public PaginationFilter()
        {
            PageNumber = 1;
            PageSize = Filter_Paramater.PageSize;
        }

        public PaginationFilter(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber < 1 ? 1 : pageNumber;
            PageSize = pageSize > 0 ? pageSize : Filter_Paramater.PageSize;
        }
    }

    public class PaginationSpecificationFilter : PaginationFilter
    {
        public string SearchText { get; set; } = string.Empty;
        public string OrderBy { get; set; } = string.Empty;
        public bool IsAddNew { get; set; } = false;

        public bool IsValidate { get; set; } = false;

        public PaginationSpecificationFilter()
            : base()
        {
        }

        public PaginationSpecificationFilter(int pageNumber, int pageSize,
            //Guid? tenantId,
            string searchText, string orderBy)
            : base(pageNumber, pageSize)
        {
            //TenantId = tenantId;
            SearchText = searchText;
            OrderBy = orderBy;
        }
    }

    public class PaginationGroupBySpecificationFilter : PaginationSpecificationFilter
    {
        public string GroupBy { get; set; }

        public PaginationGroupBySpecificationFilter()
            : base()
        {
        }

        public PaginationGroupBySpecificationFilter(int pageNumber, int pageSize,
            string searchText, string groupBy, string orderBy)
            : base(pageNumber, pageSize,
                  searchText, orderBy)
        {
            GroupBy = groupBy;
        }
    }
}
