using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Application.Interfaces
{
    public interface IPhotoStorageService
    {
        Task<string> SaveFileAsync(Stream fileStream, string fileName);
        Task DeleteFileAsync(string fileUrl);
    }
}
