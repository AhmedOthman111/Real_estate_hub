using MediatR;
using RealEstateHub.Application.CQRS.Ads.Dto;
using RealEstateHub.Application.CQRS.Ads.Queries;
using RealEstateHub.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Application.CQRS.Ads.Handlers
{
    public class GetMyAdsQueryHandler : IRequestHandler< GetMyAdsQuery , List<AdResponseDto>>
    {
        private readonly IAdService _adService;
        public GetMyAdsQueryHandler(IAdService adService)
        {
            _adService = adService;
        }
        public async Task<List<AdResponseDto>> Handle(GetMyAdsQuery request, CancellationToken cancellationToken)
        {
            return await _adService.GetMyAdsAsync(request.AUOwnerId);
        }


    }
}
