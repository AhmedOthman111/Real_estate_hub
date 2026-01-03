using MediatR;
using RealEstateHub.Application.CQRS.AdPhotos.Commands;
using RealEstateHub.Application.Interfaces;
using RealEstateHub.Infrastructure.ExternalServices;

namespace RealEstateHub.Application.CQRS.AdPhotos.Handler
{
    public class DeleteAdPhotoCommandHandler : IRequestHandler<DeleteAdPhotoCommand>
    {
        private readonly IAdPhotosService _adPhotosService;
        private readonly ICacheService _cacheService;

        public DeleteAdPhotoCommandHandler(IAdPhotosService adPhotosService, ICacheService cacheService)
        {
            _adPhotosService = adPhotosService;
            _cacheService = cacheService;
        }

        public async Task Handle(DeleteAdPhotoCommand request, CancellationToken cancellationToken)
        {
            var adId = await _adPhotosService.DeletePhotoAsync(request.PhotoId, request.AUOwnerId);
            await _cacheService.RemoveAsync($"ad:{adId}", cancellationToken);
        }
    }
}
