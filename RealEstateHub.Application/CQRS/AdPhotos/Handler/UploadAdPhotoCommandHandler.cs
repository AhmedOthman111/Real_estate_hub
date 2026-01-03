using MediatR;
using RealEstateHub.Application.CQRS.AdPhotos.Commands;
using RealEstateHub.Application.Interfaces;
using RealEstateHub.Infrastructure.ExternalServices;

namespace RealEstateHub.Application.CQRS.AdPhotos.Handler
{
    public class UploadAdPhotoCommandHandler : IRequestHandler<UploadAdPhotoCommand, string>
    {
        private readonly IAdPhotosService _adPhotosService;
        private readonly ICacheService _cacheService;

        public UploadAdPhotoCommandHandler(IAdPhotosService adPhotosService, ICacheService cacheService)
        {
            _adPhotosService = adPhotosService;
            _cacheService = cacheService;
        }

        public async Task<string> Handle(UploadAdPhotoCommand request, CancellationToken cancellationToken)
        {
            var url = await _adPhotosService.UploadPhotoAsync(request.AdId, request.FileStream, request.FileName, request.AUOwnerId);
            await _cacheService.RemoveAsync($"ad:{request.AdId}", cancellationToken);

            return url;
        }
    }
}
