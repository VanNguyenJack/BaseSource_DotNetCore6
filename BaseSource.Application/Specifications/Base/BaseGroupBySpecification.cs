using BaseSource.Application.Exceptions;
using BaseSource.Application.Wrappers;
using BaseSource.Domain;
using BaseSource.Domain.CommonFilters;
using BaseSource.Domain.Constants;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BaseSource.Application.Specifications.Base
{
    public class BaseGroupBySpecification<T, TResponse> : IGroupBySpecification<T, TResponse>
    {
        private readonly IStringLocalizer _localizer;
        public int Take { get; set; }
        public int Skip { get; set; }
        public bool IsPagingEnabled { get; set; } = false;
        public Expression<Func<T, bool>> Criteria { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();
        public Dictionary<Expression<Func<TResponse, object>>, string> OrderBy { get; private set; }
        public Dictionary<Expression<Func<TResponse, object>>, string> ThenBy { get; private set; }
        public Expression<Func<T, string>> GroupBy { get; private set; }
        public Expression<Func<string, IEnumerable<T>, TResponse>> GroupByResultHandler { get; set; }

        public BaseGroupBySpecification(IStringLocalizer localizer)
        {
        }
        public BaseGroupBySpecification(Expression<Func<T, bool>> criteria, IStringLocalizer localizer)
        {
            Criteria = criteria;
        }

        public BaseGroupBySpecification(PaginationGroupBySpecificationFilter filter, bool isPaging, IStringLocalizer localizer)
        {
            _localizer = localizer;
            CreateGroupBy(filter.GroupBy);
            CreateOrderBy(filter.OrderBy);
            if (isPaging)
            {
                IsPagingEnabled = true;
                Take = filter.PageSize;
                Skip = (filter.PageNumber - 1) * filter.PageSize;
            }
        }
        protected void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        protected void AddGroupBy(Expression<Func<T, string>> groupByExpression)
        {
            GroupBy = groupByExpression;
        }

        protected void AddOrderBy(Expression<Func<TResponse, object>> orderByExpression, string direction = "asc")
        {
            OrderBy = new Dictionary<Expression<Func<TResponse, object>>, string> { { orderByExpression, direction } };
        }

        protected void AddThenBy(Expression<Func<TResponse, object>> thenByExpression, string direction = "asc")
        {
            if (ThenBy == null)
                ThenBy = new Dictionary<Expression<Func<TResponse, object>>, string>();

            if (!ThenBy.ContainsKey(thenByExpression))
                ThenBy.Add(thenByExpression, direction);
        }

        protected void CreateGroupBy(string groupBy)
        {
            if (!string.IsNullOrEmpty(groupBy))
            {
                Type entityType = typeof(T);
                var entityProperties = entityType.GetProperties().Select(property => property.Name.ToLower());

                var arrGroupBy = groupBy.Split(",", StringSplitOptions.RemoveEmptyEntries);
                if (arrGroupBy.Length > 1)
                    throw new ApiException(new Message(MessagesKey.GroupingByMultipleColumnsNotSupported, _localizer[MessagesKey.GroupingByMultipleColumnsNotSupported], MessageType.Error));

                var groupByColumn = arrGroupBy[0].Trim().ToLower();

                if (!entityProperties.Contains(groupByColumn))
                    throw new ApiException(new Message(MessagesKey.ColumnNotSupportedForGrouping, _localizer[MessagesKey.ColumnNotSupportedForGrouping, groupByColumn], MessageType.Error));

                ApplyGroupBy(groupByColumn, AddGroupBy);

                if (GroupBy == null)
                    throw new ApiException(new Message(MessagesKey.ColumnNotSupportedForGrouping, _localizer[MessagesKey.ColumnNotSupportedForGrouping, groupByColumn], MessageType.Error));
            }
        }

        protected void CreateOrderBy(string orderBy)
        {
            if (!string.IsNullOrEmpty(orderBy))
            {
                Type entityType = typeof(TResponse);
                var entityProperties = entityType.GetProperties().Select(property => property.Name.ToLower());

                var arrOrderBy = orderBy.Split(",", StringSplitOptions.RemoveEmptyEntries);
                var orderByColumn = arrOrderBy[0].Trim().ToLower();

                var arrColumnOrderInfo = orderByColumn.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                if (!entityProperties.Contains(arrColumnOrderInfo[0])
                    || arrColumnOrderInfo.Length > 1 && !string.IsNullOrWhiteSpace(arrColumnOrderInfo[1]) && arrColumnOrderInfo[1].Trim() != "asc" && arrColumnOrderInfo[1].Trim() != "desc")
                    throw new ApiException(new Message(MessagesKey.InvalidOrderSpecified, _localizer[MessagesKey.InvalidOrderSpecified, orderByColumn], MessageType.Error));

                ApplyOrderBy(arrColumnOrderInfo, AddOrderBy);

                if (arrOrderBy.Length > 1)
                {
                    for (int i = 1; i < arrOrderBy.Length; i++)
                    {
                        var thenByColumn = arrOrderBy[i].Trim().ToLower();

                        arrColumnOrderInfo = thenByColumn.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                        if (!entityProperties.Contains(arrColumnOrderInfo[0])
                            || arrColumnOrderInfo.Length > 1 && !string.IsNullOrWhiteSpace(arrColumnOrderInfo[1]) && arrColumnOrderInfo[1].Trim() != "asc" && arrColumnOrderInfo[1].Trim() != "desc")
                            throw new ApiException(new Message(MessagesKey.InvalidOrderSpecified, _localizer[MessagesKey.InvalidOrderSpecified, thenByColumn], MessageType.Error));

                        ApplyOrderBy(arrColumnOrderInfo, AddThenBy);
                    }
                }
            }
        }

        protected virtual void ApplyGroupBy(string groupByColumn, Action<Expression<Func<T, string>>> AddAction) { }
        protected virtual void ApplyOrderBy(string[] arrColumnOrderInfo, Action<Expression<Func<TResponse, object>>, string> AddAction) { }

    }
}
