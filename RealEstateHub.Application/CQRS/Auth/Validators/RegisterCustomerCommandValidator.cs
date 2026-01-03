using FluentValidation;
using RealEstateHub.Application.CQRS.Auth.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Application.CQRS.Auth.Validators
{
    public class RegisterCustomerCommandValidator : AbstractValidator<RegisterCustomerCommand>
    {

        public RegisterCustomerCommandValidator()
        {
            RuleFor(x => x.Dto.FirstName).NotEmpty().MaximumLength(50);

            RuleFor(x => x.Dto.LastName).NotEmpty().MaximumLength(50);

            RuleFor(x => x.Dto.Email).NotEmpty().EmailAddress();

            RuleFor(x => x.Dto.UserName).NotEmpty().MinimumLength(5);
          
            RuleFor(x => x.Dto.PhoneNumber).NotEmpty().MaximumLength(11).MinimumLength(11).WithMessage("Phone Number must Be 11 digit");

            RuleFor(x => x.Dto.NationalId).NotEmpty().MinimumLength(14).MaximumLength(14).WithMessage("National Id must be 14 digit");
            
            RuleFor(x => x.Dto.Password).NotEmpty().MinimumLength(6);

            RuleFor(x => x.Dto.ConfirmPassword).Equal(x => x.Dto.Password).WithMessage("Passwords do not match");

        }

    }
}
