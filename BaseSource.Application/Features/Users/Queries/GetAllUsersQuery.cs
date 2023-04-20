using BaseSource.Application.Exceptions;
using BaseSource.Application.Wrappers;
using BaseSource.Application.Specifications;
using BaseSource.Domain;
using BaseSource.Domain.CommonFilters;
using BaseSource.Domain.Constants;
using BaseSource.Domain.DTOs.Account;
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
using BaseSource.Application.Utilities;

namespace BaseSource.Application.Features.Users.Queries
{
    public class GetAllUsersQuery : IRequest<PaginatedResult<List<UserDto>>>
    {
        public PaginationSpecificationFilter Filter { get; set; }
        public bool ExactMatch { get; set; }
        public string Route { get; set; }

        public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, PaginatedResult<List<UserDto>>>
        {
            private readonly IUriService _uriService;
            private readonly IStringLocalizer _localizer;
            private readonly IUnitOfWork _unitOfWork;
            public GetAllUsersQueryHandler(IUnitOfWork unitOfWork, IUriService uriService, IStringLocalizer localizer)
            {
                _unitOfWork = unitOfWork;
                _uriService = uriService;
                _localizer = localizer;
            }

            public async Task<PaginatedResult<List<UserDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
            {
                var searchText = request.Filter.SearchText;
                var query = (from u in _unitOfWork.Accounts.AsNoTracking()
                             where u.IsActive == Rec_Status_Account.Active && (searchText != null ? request.ExactMatch ? u.Uid.ToString() == searchText : u.FullName.Contains(searchText)
                                || u.LastName.Contains(searchText)
                                || u.Uid.ToString().Contains(searchText) : true)
                             select new UserDto
                             {
                                 Email = u.Email,
                                 Id = u.Uid,
                                 Name = u.FullName,
                                 Role = u.Type
                             }).AsEnumerable().GroupBy(e => new { e.Id, e.Name, e.Email })
                               .Select(x => x.FirstOrDefault()).AsQueryable();
                var specification = new UsersSpecification(request.Filter, _localizer, request.ExactMatch);
                int count = _unitOfWork.Repository<UserDto>().CountWithSpecificationPattern(query, specification);
                var users = _unitOfWork.Repository<UserDto>().FindWithSpecificationPattern(query, specification);
                var rs = users.Adapt<List<UserDto>>();
                if (rs.Count == 0 && string.IsNullOrEmpty(request.Filter.SearchText))
                {
                    throw new BadRequestException(new Message(MessagesKey.UserNotFound, $"User id {request.Filter.SearchText} is not exist", MessageType.Error));
                }
                return PaginationHelper.CreatePagedResponse(rs, request.Filter, count, _uriService, request.Route);
            }
        }
    }
}
