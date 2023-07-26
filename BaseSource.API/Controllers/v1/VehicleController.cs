using BaseSource.Application.Features.Users.Queries;
using BaseSource.Application.Features.Vehicles.Commands;
using BaseSource.Application.Features.Vehicles.Queries;
using BaseSource.Application.Swagger;
using BaseSource.Application.Wrappers;
using BaseSource.Domain.CommonFilters;
using BaseSource.Domain.DTOs.Account;
using BaseSource.Domain.DTOs.Vehicle;
using BaseSource.Domain.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace BaseSource.API.Controllers.v1
{
    [Authorize]
    [Route("api/v{version:apiVersion}/vehicles")]
    [ApiController]
    [ApiVersion("1.0")]
    public class VehicleController : BaseApiController<AccountController>
    {
        #region API get all user
        [HttpGet("get-all-users")]
        [SwaggerOperation("get-all-users")]
        public async Task<PaginatedResult<List<VehicleDto>>> GetAllUsers([FromQuery] PaginationSpecificationFilter filter, bool exactMatch = false)
        {
            var route = HttpContext.Request.Path.ToString();
            return await Mediator.Send(new GetAllVehicleQuery { Filter = filter, ExactMatch = exactMatch, Route = route });
        }
        #endregion

        #region API Creat or update vehicle
        [HttpPost("create-and-update-vehicle")]
        public async Task<Result<string>> CreateAndUpdateVehicleCommandAsync([FromBody] ItemVehicleCreateUpdateRequestDto itemVehicleCreateUpdateRequest, [FromQuery] bool isNew = false)
        {
            return await Mediator.Send(new CreateAndUpdateItemVehicleCommand { ItemVehicleUpdateRequest = itemVehicleCreateUpdateRequest, IsNew = isNew });
        }
        #endregion

        #region API delete vehicle
        [HttpDelete("delete-vehicle")]
        public async Task<Result<string>> DeleteVehicleCommandAsync([FromBody] int vehicleId)
        {
            return await Mediator.Send(new DeleteVehicleItemCommand { DeleteVehicleId = vehicleId });
        }
        #endregion

    }
}
