using BaseSource.Application.Wrappers;
using BaseSource.Domain.DTOs.Identity;
using MediatR;
using BaseSource.Application.Services.Authentication;
using System.Threading;
using System.Threading.Tasks;

namespace BaseSource.Application.Features.Users.Commands
{
    public class LoginCommand : IRequest<Result<TokenResponse>>
    {
        public string UserNameOrEmail { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public string IPAddress { get; set; }


        public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<TokenResponse>>
        {
            private readonly IAuthenticationService _authentication;

            public LoginCommandHandler(IAuthenticationService authentication) => _authentication = authentication;

            public async Task<Result<TokenResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
            {
                return await _authentication.LoginAsync(request);
            }
        }
    }
}
