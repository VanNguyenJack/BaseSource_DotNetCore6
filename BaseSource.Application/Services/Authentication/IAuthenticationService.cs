using BaseSource.Application.Features.Users.Commands;
using BaseSource.Application.Wrappers;
using BaseSource.Domain.DTOs.Identity;

namespace BaseSource.Application.Services.Authentication
{
    public interface IAuthenticationService
    {
        Task<Result<TokenResponse>> LoginAsync(LoginCommand request);
    }
}
