using FluentValidation;
using RealEstateHub.Application.CQRS.Auth.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Application.CQRS.Auth.Validators
{
    public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidator()
        {
            RuleFor(x => x.dto.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.dto.Otp).NotEmpty();
            RuleFor(x => x.dto.NewPassword).NotEmpty();
            RuleFor(x => x.dto.ConfirmNewPassword).Equal(x => x.dto.NewPassword).WithMessage("Passwords do not match");

        }
    }
}
