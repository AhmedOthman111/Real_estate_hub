using MediatR;
using RealEstateHub.Application.CQRS.SaveAd.Dto;
using RealEstateHub.Application.CQRS.SaveAd.Queries;
using RealEstateHub.Application.Interfaces;
using RealEstateHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Application.CQRS.SaveAd.Handler
{
    public class GetMySavedQueryHandler : IRequestHandler<GetMySavedQuery , List<SaveAdResponseDto>>
    {
        private readonly ICacheService _cacheService;
        private readonly ISaveAdService _saveAdService;
        public GetMySavedQueryHandler(ICacheService cacheService , ISaveAdService saveAdService) 
        {
            _cacheService = cacheService;
            _saveAdService = saveAdService;
        }
        public async Task<List<SaveAdResponseDto>> Handle (GetMySavedQuery request , CancellationToken cancellationToken)
        {
            var cacheKey = $"saved:customer:{request.customerId}";

            var cached = await _cacheService
                .GetAsync<List<SaveAdResponseDto>>(cacheKey, cancellationToken);

            if (cached != null)
                return cached;

            var savedAds = await _saveAdService
                .GetMySavedAsync(request.customerId);

            await _cacheService.SetAsync( cacheKey, savedAds, TimeSpan.FromMinutes(10), null, cancellationToken);

            return savedAds;

        }
    }
}
