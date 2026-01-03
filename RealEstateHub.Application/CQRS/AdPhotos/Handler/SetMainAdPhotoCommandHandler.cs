using MediatR;
using RealEstateHub.Application.Interfaces;
using RealEstateHub.Infrastructure.ExternalServices;

namespace RealEstateHub.Application.CQRS.AdPhotos.Commands
{

    public class SetMainAdPhotoCommandHandler : IRequestHandler<SetMainAdPhotoCommand>
    {
        private readonly IAdPhotosService _adPhotosService;
        private readonly ICacheService _cacheService;

        public SetMainAdPhotoCommandHandler(IAdPhotosService adService, ICacheService cacheService)
        {
            _adPhotosService = adService;
            _cacheService = cacheService;
        }

        public async Task Handle(SetMainAdPhotoCommand request, CancellationToken cancellationToken)
        {
            var adId = await _adPhotosService.SetMainPhotoAsync(request.PhotoId, request.AUOwnerId);
            await _cacheService.RemoveAsync($"ad:{adId}", cancellationToken);
        }
    }
}
