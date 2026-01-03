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
    public class GetPendingAdsHandler : IRequestHandler<GetPendingAdsQuery , IReadOnlyList<AdResponseDto>>
    {
        private readonly IAdService _adservice;
        public GetPendingAdsHandler(IAdService adService) 
        {
            _adservice = adService;
        }

        public async Task <IReadOnlyList<AdResponseDto>> Handle (GetPendingAdsQuery request , CancellationToken cancellationToken)
        {
            return await _adservice.GetAllPendingAds();
        }
    }
}
