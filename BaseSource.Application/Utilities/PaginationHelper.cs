using BaseSource.Domain.CommonFilters;
using BaseSource.Domain.Shared;
using BaseSource.Domain.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSource.Application.Utilities
{
    public static class PaginationHelper
    {
        public static PaginatedResult<List<T>> CreatePagedResponse<T>(List<T> pagedData, PaginationGroupBySpecificationFilter filter, int totalRecords, IUriService uriService, string route)
        {
            var response = new PaginatedResult<List<T>>(true, pagedData, null, 0, filter.PageNumber, filter.PageSize);
            var totalPages = totalRecords / (double)filter.PageSize;
            int roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));

            response.NextPage = filter.PageNumber >= 1 && filter.PageNumber < roundedTotalPages ? uriService.GetPageUri
                (new PaginationGroupBySpecificationFilter(filter.PageNumber + 1, filter.PageSize, filter.SearchText, filter.GroupBy, filter.OrderBy), route) : null;

            response.PreviousPage = filter.PageNumber - 1 >= 1 && filter.PageNumber <= roundedTotalPages ? uriService.GetPageUri
                (new PaginationGroupBySpecificationFilter(filter.PageNumber - 1, filter.PageSize, filter.SearchText, filter.GroupBy, filter.OrderBy), route) : null;

            response.FirstPage = uriService.GetPageUri(new PaginationGroupBySpecificationFilter(1, filter.PageSize, filter.SearchText, filter.GroupBy, filter.OrderBy), route);

            response.LastPage = uriService.GetPageUri(new PaginationGroupBySpecificationFilter(roundedTotalPages, filter.PageSize, filter.SearchText, filter.GroupBy, filter.OrderBy), route);

            response.TotalPages = roundedTotalPages;
            response.TotalRecords = totalRecords;

            response.IsValidate = filter.IsValidate;
            response.IsAddNew = filter.IsAddNew;

            return response;
        }

        public static PaginatedResult<List<T>> CreatePagedResponse<T>(List<T> pagedData, PaginationSpecificationFilter filter, int totalRecords, IUriService uriService, string route)
        {
            var response = new PaginatedResult<List<T>>(true, pagedData, null, 0, filter.PageNumber, filter.PageSize);
            var totalPages = filter.PageSize == 0 ? 1 : totalRecords / (double)filter.PageSize;
            int roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));

            response.NextPage = filter.PageNumber >= 1 && filter.PageNumber < roundedTotalPages ? uriService.GetPageUri
                (new PaginationSpecificationFilter(filter.PageNumber + 1, filter.PageSize, filter.SearchText, filter.OrderBy), route) : null;

            response.PreviousPage = filter.PageNumber - 1 >= 1 && filter.PageNumber <= roundedTotalPages ? uriService.GetPageUri
                (new PaginationSpecificationFilter(filter.PageNumber - 1, filter.PageSize, filter.SearchText, filter.OrderBy), route) : null;

            response.FirstPage = uriService.GetPageUri(new PaginationSpecificationFilter(1, filter.PageSize, filter.SearchText, filter.OrderBy), route);

            response.LastPage = uriService.GetPageUri(new PaginationSpecificationFilter(roundedTotalPages, filter.PageSize, filter.SearchText, filter.OrderBy), route);

            response.TotalPages = roundedTotalPages;
            response.TotalRecords = totalRecords;

            response.IsValidate = filter.IsValidate;
            response.IsAddNew = filter.IsAddNew;

            return response;
        }
        public static PaginatedResult<List<T>> CreatePagedResponse<T>(List<T> pagedData, PaginationFilter filter, int totalRecords, IUriService uriService, string route)
        {
            var response = new PaginatedResult<List<T>>(true, pagedData, null, 0, filter.PageNumber, filter.PageSize);
            var totalPages = totalRecords / (double)filter.PageSize;
            int roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));
            response.NextPage =
                filter.PageNumber >= 1 && filter.PageNumber < roundedTotalPages
                    ? uriService.GetPageUri(new PaginationFilter(filter.PageNumber + 1, filter.PageSize), route)
                    : null;
            response.PreviousPage =
                filter.PageNumber - 1 >= 1 && filter.PageNumber <= roundedTotalPages
                    ? uriService.GetPageUri(new PaginationFilter(filter.PageNumber - 1, filter.PageSize), route)
                    : null;
            response.FirstPage = uriService.GetPageUri(new PaginationFilter(1, filter.PageSize), route);
            response.LastPage = uriService.GetPageUri(new PaginationFilter(roundedTotalPages, filter.PageSize), route);
            response.TotalPages = roundedTotalPages;
            response.TotalRecords = totalRecords;
            return response;
        }
    }
}
