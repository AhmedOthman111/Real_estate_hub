using FluentValidation;
using RealEstateHub.Application.CQRS.Auth.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Application.CQRS.Auth.Validators
{
    public class RegisterOwnerCommandValidator : AbstractValidator<RegisterOwnerCommand>
    {
        public RegisterOwnerCommandValidator() 
        {

            RuleFor(x => x.dto.FirstName).NotEmpty().MaximumLength(15);

            RuleFor(x => x.dto.LastName).NotEmpty().MaximumLength(15);

            RuleFor(x => x.dto.Email).NotEmpty().EmailAddress();

            RuleFor(x => x.dto.UserName).NotEmpty().MinimumLength(5);

            RuleFor(x => x.dto.PhoneNumber).NotEmpty().MaximumLength(11).MinimumLength(11).WithMessage("Phone Number must Be 11 digit");

            RuleFor(x => x.dto.WhatsappNumber).NotEmpty().MaximumLength(11).MinimumLength(11).WithMessage("WhatsApp Number must Be 11 digit");

            RuleFor(x => x.dto.NationalId).NotEmpty().MinimumLength(14).MaximumLength(14).WithMessage("National Id must be 14 digit");

            RuleFor(x => x.dto.Address).NotEmpty().MaximumLength(100);

            RuleFor(x => x.dto.Bio).NotEmpty().MaximumLength(500);

            RuleFor(x => x.dto.CompanyName).NotEmpty().MaximumLength(50);

            RuleFor(x => x.dto.Password).NotEmpty().MinimumLength(6);

            RuleFor(x => x.dto.ConfirmPassword).Equal(x => x.dto.Password).WithMessage("Passwords do not match");
        }

    }
}
