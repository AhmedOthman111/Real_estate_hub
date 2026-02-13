using RealEstateHub.Application.CQRS.SaveAd.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Application.Interfaces
{
    public interface ISaveAdService
    {
        Task ToggleSaveAsync(int adId, string customerId);
        Task<List<SaveAdResponseDto>> GetMySavedAsync(string customerId);

    }
}
