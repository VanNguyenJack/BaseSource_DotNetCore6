using BaseSource.Application.Specifications.Base;
using BaseSource.Domain.CommonFilters;
using BaseSource.Domain.DTOs.Account;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BaseSource.Application.Specifications
{
    public class UsersSpecification : BaseSpecification<UserDto>
    {
        public UsersSpecification(PaginationSpecificationFilter filter, IStringLocalizer localizer, bool exactMatch) : base(filter, localizer)
        {
            var predicate = PredicateBuilder.True<UserDto>();
            Criteria = predicate;
        }

        protected override void ApplyOrderBy(string[] arrColumnOrderInfo, Action<Expression<Func<UserDto, object>>, string> AddAction)
        {
            var column = arrColumnOrderInfo[0];
            var direction = arrColumnOrderInfo.Length > 1 ? arrColumnOrderInfo[1] : "asc";

            switch (column)
            {
                case "uid":
                    AddAction(x => x.Id, direction);
                    break;
                case "fullname":
                    AddAction(x => x.Name, direction);
                    break;
                default: break;
            }
        }
    }
}
