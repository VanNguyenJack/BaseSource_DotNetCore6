using BaseSource.Application.Features.Users.Commands;
using BaseSource.Application.Wrappers;
using BaseSource.Domain.DTOs.Identity;
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
    }
}
