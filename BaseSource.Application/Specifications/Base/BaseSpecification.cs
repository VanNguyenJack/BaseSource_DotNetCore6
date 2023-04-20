using BaseSource.Application.Exceptions;
using BaseSource.Application.Wrappers;
using BaseSource.Domain.CommonFilters;
using BaseSource.Domain.Constants;
using BaseSource.Domain;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BaseSource.Application.Specifications.Base
{
    public class BaseSpecification<T> : ISpecification<T>
    {
        private readonly IStringLocalizer _localizer;
        public BaseSpecification(IStringLocalizer localizer)
        {
            _localizer = localizer;
        }

        public BaseSpecification(Expression<Func<T, bool>> criteria, IStringLocalizer localizer)
        {
            _localizer = localizer;
            Criteria = criteria;
        }

        public BaseSpecification(PaginationSpecificationFilter filter, IStringLocalizer localizer)
        {
            _localizer = localizer;
            bool isPaging = filter.PageNumber > 0 || filter.PageSize > 0;
            CreateOrderBy(filter.OrderBy);
            if (isPaging)
            {
                IsPagingEnabled = true;
                Take = filter.PageSize;
                Skip = (filter.PageNumber - 1) * filter.PageSize;
            }

            if (filter.IsValidate)
            {
                Take = 1;
                Skip = 0;
            }
        }

        public int Take { get; set; }
        public int Skip { get; set; }
        public bool IsPagingEnabled { get; set; } = false;
        public Expression<Func<T, bool>> Criteria { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();
        public Dictionary<Expression<Func<T, object>>, string> OrderBy { get; private set; }
        public Dictionary<Expression<Func<T, object>>, string> ThenBy { get; private set; }

        protected void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        protected void AddOrderBy(Expression<Func<T, object>> orderByExpression, string direction = "asc")
        {
            OrderBy = new Dictionary<Expression<Func<T, object>>, string> { { orderByExpression, direction } };
        }

        protected void CreateOrderBy(string orderBy)
        {
            if (!string.IsNullOrEmpty(orderBy))
            {
                Type entityType = typeof(T);
                var entityProperties = GetAllProperties(entityType);

                var arrOrderBy = orderBy.ToLower().Split(",", StringSplitOptions.RemoveEmptyEntries);
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

        protected void AddThenBy(Expression<Func<T, object>> thenByExpression, string direction = "asc")
        {
            if (ThenBy == null)
                ThenBy = new Dictionary<Expression<Func<T, object>>, string>();

            if (!ThenBy.ContainsKey(thenByExpression))
                ThenBy.Add(thenByExpression, direction);
        }

        protected virtual void ApplyOrderBy(string[] arrColumnOrderInfo, Action<Expression<Func<T, object>>, string> AddAction) { }

        private List<string> GetAllProperties(Type type)
        {
            var validProperties = new List<string>();
            var typeProperties = type.GetProperties();

            validProperties.AddRange(typeProperties.Where(x => !x.GetGetMethod().IsVirtual).Select(property => property.Name.ToLower()));

            var virtualProperties = typeProperties.Where(x => x.GetGetMethod().IsVirtual);
            foreach (var property in virtualProperties)
            {
                var childProperties = property.PropertyType.GetProperties();
                validProperties.AddRange(childProperties.Where(x => !x.GetGetMethod().IsVirtual).Select(childProperty => $"{property.Name.ToLower()}.{childProperty.Name.ToLower()}"));
            }

            return validProperties;
        }
    }
}
