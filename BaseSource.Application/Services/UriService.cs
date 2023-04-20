using BaseSource.Domain.CommonFilters;
using BaseSource.Domain.Constants;
using BaseSource.Domain.Shared;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSource.Application.Services
{
    public class UriService : IUriService
    {
        private readonly string _baseUri;

        public UriService(string baseUri)
        {
            _baseUri = baseUri;
        }

        public Uri GetPageUri(PaginationFilter filter, string route)
        {
            var endPointUri = new Uri(string.Concat(_baseUri, route));
            var modifiedUri =
                QueryHelpers.AddQueryString(endPointUri.ToString(), Filter_Paramater.UriPageNumber, filter.PageNumber.ToString());
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, Filter_Paramater.UriPageSize, filter.PageSize.ToString());
            return new Uri(modifiedUri);
        }

        public Uri GetPageUri(PaginationSpecificationFilter filter, string route)
        {
            var endPointUri = new Uri(string.Concat(_baseUri, route));

            var modifiedUri = QueryHelpers.AddQueryString(endPointUri.ToString(), Filter_Paramater.UriPageNumber, filter.PageNumber.ToString());

            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, Filter_Paramater.UriPageSize, filter.PageSize.ToString());

            if (!string.IsNullOrEmpty(filter.SearchText))
                modifiedUri = QueryHelpers.AddQueryString(modifiedUri, Filter_Paramater.UriSearchText, filter.SearchText.ToString());

            if (!string.IsNullOrEmpty(filter.OrderBy))
                modifiedUri = QueryHelpers.AddQueryString(modifiedUri, Filter_Paramater.UriOrderBy, filter.OrderBy.ToString());

            return new Uri(modifiedUri);
        }

        public Uri GetPageUri(PaginationGroupBySpecificationFilter filter, string route)
        {
            var endPointUri = new Uri(string.Concat(_baseUri, route));

            var modifiedUri = QueryHelpers.AddQueryString(endPointUri.ToString(), Filter_Paramater.UriPageNumber, filter.PageNumber.ToString());

            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, Filter_Paramater.UriPageSize, filter.PageSize.ToString());

            if (!string.IsNullOrEmpty(filter.SearchText))
                modifiedUri = QueryHelpers.AddQueryString(modifiedUri, Filter_Paramater.UriSearchText, filter.SearchText.ToString());

            if (!string.IsNullOrEmpty(filter.GroupBy))
                modifiedUri = QueryHelpers.AddQueryString(modifiedUri, Filter_Paramater.UriGroupBy, filter.GroupBy.ToString());

            if (!string.IsNullOrEmpty(filter.OrderBy))
                modifiedUri = QueryHelpers.AddQueryString(modifiedUri, Filter_Paramater.UriOrderBy, filter.OrderBy.ToString());

            return new Uri(modifiedUri);
        }
    }
}
