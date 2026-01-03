using FluentValidation;
using RealEstateHub.Application.CQRS.Auth.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Application.CQRS.Auth.Validators
{
    public class ConfirmEmailCommandValidator : AbstractValidator<ConfirmEmailCommand>
    {
        public ConfirmEmailCommandValidator()
        {
            RuleFor(x => x.dto.UserEmail).NotEmpty().EmailAddress();
            RuleFor(x => x.dto.Token).NotEmpty();
        }
    }
}
