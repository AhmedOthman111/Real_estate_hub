using FluentValidation;
using RealEstateHub.Application.CQRS.Auth.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Application.CQRS.Auth.Validators
{
    public class GoogleLoginCommandValidator : AbstractValidator<GoogleLoginCommand>
    {
        public GoogleLoginCommandValidator()
        {
            RuleFor(x => x.Dto.IdToken).NotEmpty();
            RuleFor(x => x.Dto.role).NotEmpty();
        }
    }
}
