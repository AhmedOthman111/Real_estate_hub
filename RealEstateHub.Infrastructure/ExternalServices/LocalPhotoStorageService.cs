using Microsoft.AspNetCore.Hosting;
using RealEstateHub.Application.Interfaces;

namespace RealEstateHub.Infrastructure.ExternalServices
{
    public class LocalPhotoStorageService : IPhotoStorageService
    {
        private readonly IWebHostEnvironment _env;

        public LocalPhotoStorageService(IWebHostEnvironment webHostEnvironment)
        {
            _env = webHostEnvironment;
        }

        public async Task<string> SaveFileAsync(Stream fileStream, string fileName)
        {
            var uploadsFolder = Path.Combine(_env.WebRootPath, "images", "ads");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }


            var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
            var filepath = Path.Combine(uploadsFolder, uniqueFileName);


            using (var fileStreamOutput = new FileStream(filepath, FileMode.Create))
            {
                await fileStream.CopyToAsync(fileStreamOutput);

            }

            return $"/images/ads/{uniqueFileName}";
        }

        public Task DeleteFileAsync(string fileUrl)
        {
            var filePath = Path.Combine(_env.WebRootPath, fileUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            return Task.CompletedTask;
        }


    }
}
