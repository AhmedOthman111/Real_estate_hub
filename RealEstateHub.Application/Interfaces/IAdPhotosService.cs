namespace RealEstateHub.Infrastructure.ExternalServices
{
    public interface IAdPhotosService
    {
        Task<string> UploadPhotoAsync(int adId, Stream fileStream, string fileName, string AUOwnerId);
        Task<int> DeletePhotoAsync(int photoId, string AUOwnerId);
        Task<int> SetMainPhotoAsync(int photoId, string AUOwnerId);

    }
}