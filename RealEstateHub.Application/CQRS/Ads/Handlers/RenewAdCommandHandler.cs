using MediatR;
using RealEstateHub.Application.CQRS.Ads.Commands;
using RealEstateHub.Application.Interfaces;

namespace RealEstateHub.Application.CQRS.Ads.Handlers
{
    public class RenewAdCommandHandler : IRequestHandler<RenewAdCommand>
    {
        private readonly IAdService _adService;
        private readonly ICacheService _cacheService;
        public RenewAdCommandHandler(ICacheService cacheService, IAdService adService)
        {
            _cacheService = cacheService;
            _adService = adService;
        }
        public async Task Handle(RenewAdCommand request, CancellationToken cancellationToken)
        {
            await _adService.RenewAdAsync(request.id, request.DurationDays, request.AUOwnerId);
            await _cacheService.RemoveAsync($"ad:{request.id}", cancellationToken);
        }
    }
}
