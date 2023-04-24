using BaseSource.Application.Specifications.Base;
using BaseSource.Domain.CommonFilters;
using BaseSource.Domain.DTOs.Account;
using BaseSource.Domain.DTOs.Vehicle;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BaseSource.Application.Specifications
{
    public class VehicleSpecification : BaseSpecification<VehicleDto>
    {
        public VehicleSpecification(PaginationSpecificationFilter filter, IStringLocalizer localizer, bool exactMatch) : base(filter, localizer)
        {
            var predicate = PredicateBuilder.True<VehicleDto>();
            Criteria = predicate;
        }

        protected override void ApplyOrderBy(string[] arrColumnOrderInfo, Action<Expression<Func<VehicleDto, object>>, string> AddAction)
        {
            var column = arrColumnOrderInfo[0];
            var direction = arrColumnOrderInfo.Length > 1 ? arrColumnOrderInfo[1] : "asc";

            switch (column)
            {
                case "vehiclePlateNumber":
                    AddAction(x => x.VehiclePlateNumber, direction);
                    break;
                default: break;
            }
        }
    }
}
