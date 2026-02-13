using MediatR;
using RealEstateHub.Application.CQRS.SaveAd.Command;
using RealEstateHub.Application.Interfaces;

namespace RealEstateHub.Application.CQRS.SaveAd.Handler
{
    public class ToggleSaveAdHandler : IRequestHandler<ToggleSaveAdCommand>
    {
        private readonly ISaveAdService _saveAdService;
        private readonly ICacheService _cacheService;
        public ToggleSaveAdHandler(ISaveAdService saveAdService, ICacheService cacheService)
        {
            _saveAdService = saveAdService;
            _cacheService = cacheService;
        }

        public async Task Handle(ToggleSaveAdCommand request, CancellationToken cancellationToken)
        {
            await _saveAdService.ToggleSaveAsync(request.adId, request.customerId);

            var cacheKey = $"saved:customer:{request.customerId}";
            await _cacheService.RemoveAsync(cacheKey);
        }
    }
}
