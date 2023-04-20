using BaseSource.Application.Features.Users.Commands;
using BaseSource.Application.Features.Users.Queries;
using BaseSource.Application.Swagger;
using BaseSource.Application.Wrappers;
using BaseSource.Domain.CommonFilters;
using BaseSource.Domain.DTOs.Account;
using BaseSource.Domain.DTOs.Identity;
using BaseSource.Domain.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace BaseSource.API.Controllers.v1
{
    [Authorize]
    [Route("api/v{version:apiVersion}/accounts")]
    [ApiController]
    [ApiVersion("1.0")]
    public class AccountController : BaseApiController<AccountController>
    {

        #region API login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<Result<TokenResponse>> LoginAsync(LoginCommand command)
        {
            command.IPAddress = GenerateIPAddress();
            return await Mediator.Send(command);
        }

        private string GenerateIPAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                return Request.Headers["X-Forwarded-For"];
            }
            else
            {
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            }
        }
        #endregion

        #region API get all user
        [HttpGet("get-all-users")]
        [SwaggerOperation("get-all-users")]
        public async Task<PaginatedResult<List<UserDto>>> GetAllUsers([FromQuery] PaginationSpecificationFilter filter, bool exactMatch = false)
        {
            var route = HttpContext.Request.Path.ToString();
            return await Mediator.Send(new GetAllUsersQuery { Filter = filter, ExactMatch = exactMatch, Route = route });
        }
        #endregion
    }
}
