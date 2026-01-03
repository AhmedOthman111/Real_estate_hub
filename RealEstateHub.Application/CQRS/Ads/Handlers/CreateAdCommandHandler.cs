using MediatR;
using RealEstateHub.Application.CQRS.Ads.Commands;
using RealEstateHub.Application.CQRS.Ads.Dto;
using RealEstateHub.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Application.CQRS.Ads.Handlers
{
    public class CreateAdCommandHandler : IRequestHandler<CreateAdCommand , AdResponseDto>
    {
        private readonly IAdService _adServicel;
        public CreateAdCommandHandler(IAdService adService)
        {
            _adServicel = adService;
        } 

        public async Task<AdResponseDto> Handle (CreateAdCommand request , CancellationToken cancellationToken)
        {
            return await _adServicel.CreateAdAsync(request.dto, request.AUOwnerId);
        }
    }
}
