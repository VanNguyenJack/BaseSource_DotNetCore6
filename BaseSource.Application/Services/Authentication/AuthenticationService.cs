using BaseSource.Application.Exceptions;
using BaseSource.Application.Features.Users.Commands;
using BaseSource.Application.Settings;
using BaseSource.Application.Validator;
using BaseSource.Application.Wrappers;
using BaseSource.Domain.Constants;
using BaseSource.Domain.DTOs.Identity;
using BaseSource.Domain;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BaseSource.Domain.Catalog;
using Microsoft.IdentityModel.Tokens;

namespace BaseSource.Application.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtSettings _jwtSettings;
        private readonly IStringLocalizer _localizer;
        //private readonly IGlbAlertService _glbAlertService;

        public AuthenticationService(IUnitOfWork unitOfWork, IOptions<JwtSettings> jwtSettings,
             IStringLocalizer localizer)
        {
            _unitOfWork = unitOfWork;
            _jwtSettings = jwtSettings.Value;
            _localizer = localizer;
        }

        public async Task<Result<TokenResponse>> LoginAsync(LoginCommand request)
        {
            var validation = new LoginValidator(_localizer);
            var validationResult = await validation.ValidateAsync(request, CancellationToken.None);
            if (validationResult.Errors.Count > 0)
            {
                throw new FluentValidation.ValidationException(validationResult.Errors);
            }

            var existingUser = await _unitOfWork.Accounts.GetUserByEmail(request.UserNameOrEmail);
            if (existingUser == null)
            {
                throw new NotFoundException(new Message(MessagesKey.NoAccount, _localizer[MessagesKey.NoAccount, request.UserNameOrEmail], MessageType.Error));
            }

            if (!existingUser.HashedPassword.Equals(request.Password))
            {
                throw new NotFoundException(new Message(MessagesKey.InvalidCredentials, _localizer[MessagesKey.InvalidCredentials, request.UserNameOrEmail], MessageType.Error));
            }

            if (existingUser.IsActive != Rec_Status_Account.Active)
            {
                throw new NotFoundException(new Message(MessagesKey.AccountInactive, _localizer[MessagesKey.AccountInactive, request.UserNameOrEmail], MessageType.Error));
            }

            var response = await ProcessLogin(existingUser, request.IPAddress);

            var messages = new List<Message> { new Message(MessagesKey.Authenticated, _localizer[MessagesKey.Authenticated, request.UserNameOrEmail]) };
            return Result<TokenResponse>.Success(response, messages);
        }

        private async Task<TokenResponse> ProcessLogin(Account existingUser, string ip, bool isLoginGetSupervisor = false, bool isShowException = false)
        {

            var response = await GetTokenResponse(existingUser, ip);

            return response;
        }

        #region  Generate JW Token
        private async Task<TokenResponse> GetTokenResponse(Account user, string ipAddress)
        {
            JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user, ipAddress);
            TokenResponse response = new TokenResponse
            {
                Id = user.Uid,
                JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                IssuedOn = jwtSecurityToken.ValidFrom.ToLocalTime(),
                ExpiresOn = jwtSecurityToken.ValidTo.ToLocalTime(),
                Email = user.Email,
            };
            return response;
        }

        private Task<JwtSecurityToken> GenerateJWToken(Account user, string ipAddress)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Uid.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email??""),
                new Claim("UserId",user.Uid.ToString()),
                new Claim(JwtRegisteredClaimNames.GivenName, user.FullName),
                new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
                new Claim("full_name", $"{user.FullName}"),
                new Claim("ip_dddress", ipAddress)
            };
            return Task.FromResult(JWTGeneration(claims));
        }

        private JwtSecurityToken JWTGeneration(IEnumerable<Claim> claims)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }
        #endregion


    }
}
