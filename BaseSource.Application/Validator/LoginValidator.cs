using BaseSource.Application.Features.Users.Commands;
using BaseSource.Domain.Constants;
using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSource.Application.Validator
{
    public class LoginValidator : AbstractValidator<LoginCommand>
    {
        private readonly IStringLocalizer _localizer;


        public LoginValidator(IStringLocalizer localizer)
        {
            _localizer = localizer;

            RuleFor(p => p.UserNameOrEmail)
                .NotEmpty().WithMessage(_localizer[MessagesKey.PropertyRequired].Value);

            RuleFor(p => p.Password).NotEmpty().WithMessage(_localizer[MessagesKey.PropertyRequired].Value);
        }
    }
}
