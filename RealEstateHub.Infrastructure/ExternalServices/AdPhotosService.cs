using RealEstateHub.Application.Exceptions;
using RealEstateHub.Application.Interfaces;
using RealEstateHub.Domain.Entities;

namespace RealEstateHub.Infrastructure.ExternalServices
{
    public class AdPhotosService : IAdPhotosService
    {
        private readonly IPhotoStorageService _photoStorageService;
        private readonly IUnitOfWork _uow;

        public AdPhotosService(IPhotoStorageService photoStorageService, IUnitOfWork uow)
        {
            _photoStorageService = photoStorageService;
            _uow = uow;

        }

        public async Task<string> UploadPhotoAsync(int adId, Stream fileStream, string fileName, string AUOwnerId)
        {
            var adWithPhotos = await _uow.Ad.GetAdWithPhotos(adId);

            var owner = await _uow.Owner.GetByAppUserIdAsync(AUOwnerId);

            if (adWithPhotos.OwnerId != owner.Id)
                throw new BusinessException("You are not authorized to add photos to this ad.");


            var photoUrl = await _photoStorageService.SaveFileAsync(fileStream, fileName);

            var isFirstPhoto = !adWithPhotos.Photos.Any();

            var photo = new AdPhoto
            {
                ImageUrl = photoUrl,
                AdId = adId,
                IsMain = isFirstPhoto
            };

            await _uow.AdPhotosRepo.AddAsync(photo);
            await _uow.SaveChangesAsync();


            return photoUrl;
        }

        public async Task<int> DeletePhotoAsync(int photoId, string AUOwnerId)
        {

            var photo = await _uow.AdPhotosRepo.GetByIdAsync(photoId);


            var ad = await _uow.Ad.GetByIdAsync(photo.AdId);
            var owner = await _uow.Owner.GetByAppUserIdAsync(AUOwnerId);

            if (ad.OwnerId != owner.Id)
                throw new BusinessException("You are not authorized to delete this photo.");

            await _photoStorageService.DeleteFileAsync(photo.ImageUrl);

            _uow.AdPhotosRepo.Delete(photo);

            if (photo.IsMain && ad.Photos.Count > 1)
            {
                var otherPhoto = await _uow.AdPhotosRepo.FindAsync(p => p.AdId == photo.AdId && p.Id != photoId);
                var newMain = otherPhoto
                    .OrderBy(p => p.Id)
                    .FirstOrDefault();

                if (newMain != null)
                {
                    newMain.IsMain = true;
                    _uow.AdPhotosRepo.Update(newMain);
                }
            }

            await _uow.SaveChangesAsync();

            return ad.Id;
        }

        public async Task<int> SetMainPhotoAsync(int photoId, string AUOwnerId)
        {
            var photo = await _uow.AdPhotosRepo.GetByIdAsync(photoId);

            if (photo.IsMain)
                return 0;

            var ad = await _uow.Ad.GetByIdAsync(photo.AdId);
            var owner = await _uow.Owner.GetByAppUserIdAsync(AUOwnerId);

            if (ad.OwnerId != owner.Id)
                throw new BusinessException("You are not authorized to modify this photo.");


            var currentMain = (await _uow.AdPhotosRepo.FindAsync(p => p.AdId == ad.Id && p.IsMain)).FirstOrDefault();
            if (currentMain != null)
            {
                currentMain.IsMain = false;
                _uow.AdPhotosRepo.Update(currentMain);
            }

            photo.IsMain = true;
            _uow.AdPhotosRepo.Update(photo);
            await _uow.SaveChangesAsync();

            return ad.Id;

        }
    }
}
