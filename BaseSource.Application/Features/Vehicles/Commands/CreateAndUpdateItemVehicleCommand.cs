using BaseSource.Application.Wrappers;
using BaseSource.Domain;
using BaseSource.Domain.Catalog;
using BaseSource.Domain.Constants;
using BaseSource.Domain.DTOs.Vehicle;
using BaseSource.Domain.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BaseSource.Application.Features.Vehicles.Commands
{
    public class CreateAndUpdateItemVehicleCommand : IRequest<Result<string>>
    {
        public ItemVehicleCreateUpdateRequestDto ItemVehicleUpdateRequest { get; set; }
        public bool IsNew { get; set; }

        public class CreateAndUpdateItemVehicleCommandHandler : IRequestHandler<CreateAndUpdateItemVehicleCommand, Result<string>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IStringLocalizer _localizer;
            private readonly IGlbAlertService _glbAlertService;
            private readonly ISessionService _sessionService;

            public CreateAndUpdateItemVehicleCommandHandler(IUnitOfWork unitOfWork, IStringLocalizer localizer, IGlbAlertService glbAlertService, ISessionService sessionService)
            {
                _unitOfWork = unitOfWork;
                _localizer = localizer;
                _glbAlertService = glbAlertService;
                _sessionService = sessionService;
            }

            public async Task<Result<string>> Handle(CreateAndUpdateItemVehicleCommand request, CancellationToken cancellationToken)
            {
                var serverDateTime = _unitOfWork.GetServerTime();
                var itemRequest = request.ItemVehicleUpdateRequest;
                try
                {
                    var itemMasters = new Domain.Catalog.Vehicle();
                    if (!request.IsNew)
                    {
                        itemMasters = await _unitOfWork.Repository<Domain.Catalog.Vehicle>().AsNoTracking()
                                                       .FirstOrDefaultAsync(x => x.VehiclePlateNumber.ToLower() == request.ItemVehicleUpdateRequest.VehiclePlateNumber.ToLower());
                    }

                    var dataRequest = itemRequest.ToEntity(itemMasters);

                    if (request.IsNew)
                    {
                        dataRequest.CreateBy = int.Parse(_sessionService.GetCurrentUserId());
                        dataRequest.CreateDate = serverDateTime;
                        await _unitOfWork.Repository<Domain.Catalog.Vehicle>().AddAsync(dataRequest);
                    }
                    else
                    {
                        await _unitOfWork.Repository<Domain.Catalog.Vehicle>().UpdateAsync(dataRequest);
                    }
                    await _unitOfWork.Commit(cancellationToken);
                    return Result<string>.Success(await _glbAlertService.GetMessageAsync(GlbAlertConstants.Vehicle_Successfully, dataRequest.Uid));
                }
                catch(Exception ex)
                {
                    await _unitOfWork.Rollback(cancellationToken);
                    return Result<string>.Fail(null, new Message(MessagesKey.Exception, ex, MessageType.Error));
                }
            }
        }
    }
}
