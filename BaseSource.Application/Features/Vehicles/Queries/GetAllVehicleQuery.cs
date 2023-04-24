using BaseSource.Application.Exceptions;
using BaseSource.Application.Features.Users.Queries;
using BaseSource.Application.Specifications;
using BaseSource.Application.Utilities;
using BaseSource.Application.Wrappers;
using BaseSource.Domain;
using BaseSource.Domain.CommonFilters;
using BaseSource.Domain.Constants;
using BaseSource.Domain.DTOs.Account;
using BaseSource.Domain.DTOs.Vehicle;
using BaseSource.Domain.Shared;
using BaseSource.Domain.Wrappers;
using Mapster;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSource.Application.Features.Vehicles.Queries;
public record GetAllVehicleQuery : IRequest<PaginatedResult<List<VehicleDto>>>
{
    public PaginationSpecificationFilter Filter { get; set; }
    public bool ExactMatch { get; set; }
    public string Route { get; set; }
}