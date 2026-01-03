using MediatR;
using RealEstateHub.Application.Common;
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
    public class GetAdsQueryHandler : IRequestHandler<GetAdsQuery, PaginatedList<AdResponseDto>>
    {
        
        private readonly IAdService _adService;
        public GetAdsQueryHandler(IAdService adService)
        {
            _adService = adService;
        }

        public async Task<PaginatedList<AdResponseDto>> Handle(GetAdsQuery request, CancellationToken cancellationToken)
        {
            return await _adService.GetAdsAsync(request.dto);
        }

    }
}
