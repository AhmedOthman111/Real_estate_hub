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
    public class UpdateAdCommandHandler : IRequestHandler<UpdateAdCommand , AdResponseDto>
    {
        private readonly IAdService _adService;
        private readonly ICacheService _cacheService;

        public UpdateAdCommandHandler(IAdService adService, ICacheService cacheService)
        {
            _adService = adService;
            _cacheService = cacheService;
        }

        public async Task<AdResponseDto> Handle(UpdateAdCommand request, CancellationToken cancellationToken)
        {
            var result = await  _adService.UpdateAdAsync(request.dto, request.AUOwnerId);
            await _cacheService.RemoveAsync($"ad:{request.dto.Id}", cancellationToken);
            return result;
        }

    }
}
