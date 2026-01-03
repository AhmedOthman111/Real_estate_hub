using MediatR;
using RealEstateHub.Application.CQRS.Ads.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Application.CQRS.Ads.Commands
{
    public record CreateAdCommand(CreateAdDto dto , string AUOwnerId) : IRequest<AdResponseDto>;
    
}
