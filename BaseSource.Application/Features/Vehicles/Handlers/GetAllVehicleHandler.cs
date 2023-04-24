using BaseSource.Application.Exceptions;
using BaseSource.Application.Features.Vehicles.Queries;
using BaseSource.Application.Specifications;
using BaseSource.Application.Utilities;
using BaseSource.Application.Wrappers;
using BaseSource.Domain.Constants;
using BaseSource.Domain.DTOs.Vehicle;
using BaseSource.Domain.Shared;
using BaseSource.Domain.Wrappers;
using BaseSource.Domain;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mapster;

namespace BaseSource.Application.Features.Vehicles.Handlers
{
    public class GetAllVehicleQueryHandler : IRequestHandler<GetAllVehicleQuery, PaginatedResult<List<VehicleDto>>>
    {
        private readonly IUriService _uriService;
        private readonly IStringLocalizer _localizer;
        private readonly IUnitOfWork _unitOfWork;

        public GetAllVehicleQueryHandler(IUnitOfWork unitOfWork, IUriService uriService, IStringLocalizer localizer)
        {
            _unitOfWork = unitOfWork;
            _uriService = uriService;
            _localizer = localizer;
        }

        public async Task<PaginatedResult<List<VehicleDto>>> Handle(GetAllVehicleQuery request, CancellationToken cancellationToken)
        {
            var searchText = request.Filter.SearchText;
            var query = (from u in _unitOfWork.Vehicles.AsNoTracking()
                         where (searchText != null ? request.ExactMatch ? u.Uid.ToString() == searchText : u.VehiclePlateNumber.Contains(searchText)
                              || u.DriverId.ToString().Contains(searchText)
                              || u.Uid.ToString().Contains(searchText) : true)
                         select new VehicleDto
                         {
                             VehiclePlateNumber = u.VehiclePlateNumber,
                             DriverId = u.DriverId
                         }).AsEnumerable().GroupBy(e => new { e.VehiclePlateNumber, e.DriverId })
                           .Select(x => x.FirstOrDefault()).AsQueryable();
            var specification = new VehicleSpecification(request.Filter, _localizer, request.ExactMatch);
            int count = _unitOfWork.Repository<VehicleDto>().CountWithSpecificationPattern(query, specification);
            var users = _unitOfWork.Repository<VehicleDto>().FindWithSpecificationPattern(query, specification);
            var rs = users.Adapt<List<VehicleDto>>();
            if (rs.Count == 0 && string.IsNullOrEmpty(request.Filter.SearchText))
            {
                throw new BadRequestException(new Message(MessagesKey.UserNotFound, $"Vehicle id {request.Filter.SearchText} is not exist", MessageType.Error));
            }
            return PaginationHelper.CreatePagedResponse(rs, request.Filter, count, _uriService, request.Route);
        }
    }
}
