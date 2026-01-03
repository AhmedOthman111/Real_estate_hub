using FluentValidation;
using RealEstateHub.Application.CQRS.Ads.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Application.CQRS.Ads.Validators
{
    public class CreateAdCommandValidator : AbstractValidator<CreateAdCommand>
    {
        public CreateAdCommandValidator()
        {
            RuleFor(x => x.dto.Title).NotEmpty().MaximumLength(200);
            RuleFor(x => x.dto.Description).NotEmpty().MaximumLength(2000);
            RuleFor(x => x.dto.Price).GreaterThan(0);
            RuleFor(x => x.dto.AreaSize).GreaterThan(0);
            RuleFor(x => x.dto.City).NotEmpty().MaximumLength(50);
            RuleFor(x => x.dto.Area).NotEmpty().MaximumLength(150);
            RuleFor(x => x.dto.Address).NotEmpty().MaximumLength(300);
            RuleFor(x => x.dto.DurationDays).GreaterThan(0);
            RuleFor(x => x.dto.CategoryId).NotEmpty();
        }
    }
}
