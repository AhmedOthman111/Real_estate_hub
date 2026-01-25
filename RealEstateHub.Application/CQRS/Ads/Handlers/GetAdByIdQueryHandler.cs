using MediatR;
using RealEstateHub.Application.CQRS.Ads.Dto;
using RealEstateHub.Application.CQRS.Ads.Queries;
using RealEstateHub.Application.Interfaces;

namespace RealEstateHub.Application.CQRS.Ads.Handlers
{
    public class GetAdByIdQueryHandler : IRequestHandler<GetAdByIdQuery, AdWithAllDetailsDto>
    {
        private readonly IAdService _adService;
        private readonly ICacheService _cacheService;

        public GetAdByIdQueryHandler(IAdService adService, ICacheService cacheService)
        {
            _adService = adService;
            _cacheService = cacheService;
        }

        public async Task<AdWithAllDetailsDto> Handle(GetAdByIdQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"ad:{request.id}";

            var cachedAd = await _cacheService.GetAsync<AdWithAllDetailsDto>(cacheKey, cancellationToken);
            if (cachedAd != null) return cachedAd;


            var ad = await _adService.GetAdByIdAsync(request.id);
            await _cacheService.SetAsync(cacheKey, ad, TimeSpan.FromMinutes(10), null, cancellationToken);
            return ad;

        }

    }
}
