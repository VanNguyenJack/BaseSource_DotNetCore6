using BaseSource.Application.Wrappers;
using BaseSource.Domain.Services;
using BaseSource.Domain;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BaseSource.Domain.Constants;

namespace BaseSource.Application.Features.Vehicles.Commands
{
    public class DeleteVehicleItemCommand : IRequest<Result<string>>
    {
        public int DeleteVehicleId { get; set; }

        public class DeleteVehicleItemCommandHandler : IRequestHandler<DeleteVehicleItemCommand, Result<string>>
        {

            private readonly IUnitOfWork _unitOfWork;
            private readonly IStringLocalizer _localizer;
            private readonly IGlbAlertService _glbAlertService;
            private readonly ISessionService _currentUserService;

            public DeleteVehicleItemCommandHandler(IUnitOfWork unitOfWork, IStringLocalizer localizer, IGlbAlertService glbAlertService, ISessionService sessionService)
            {
                _unitOfWork = unitOfWork;
                _localizer = localizer;
                _glbAlertService = glbAlertService;
                _currentUserService = sessionService;
            }

            public async Task<Result<string>> Handle(DeleteVehicleItemCommand request, CancellationToken cancellationToken)
            {
                var itemDelete = await _unitOfWork.Repository<Domain.Catalog.Vehicle>().AsNoTracking()
                                                  .Where(x => x.Uid == request.DeleteVehicleId)
                                                  .FirstOrDefaultAsync();

                if(itemDelete != null)
                {
                    await _unitOfWork.Repository<Domain.Catalog.Vehicle>().DeleteAsync(itemDelete);

                    await _unitOfWork.Commit(cancellationToken);
                    return Result<string>.Success(await _glbAlertService.GetMessageAsync(GlbAlertConstants.Vehicle_Delete_Successfully, request.DeleteVehicleId));

                }
                return Result<string>.Fail();
            }
        }
    }
}
