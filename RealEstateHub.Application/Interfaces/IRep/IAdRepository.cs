using RealEstateHub.Application.Common;
using RealEstateHub.Application.CQRS.Ads.Dto;
using RealEstateHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Application.Interfaces.IRep
{
    public interface IAdRepository : IGenericRepository<Ad>
    {
        Task<IEnumerable<Ad>> GetActiveAdsAsync();
        Task<Ad?> GetAdWithPhotos(int id);
        Task<IReadOnlyList<Ad>> GetAllOwnerAds(int ownerId);
        Task<PaginatedList<Ad>> GetFilteredAdsAsync(AdFilterDto filter);
        Task<IReadOnlyList<Ad>> GetPendingAdsAsync();
        Task<Ad?> GetAdWithOwner(int id);
        Task<List<Ad>> GetExpiredAds();
        Task<List<Ad>> GetAdsExpireTommrow();
        Task<Ad> GetAdWithAllData(int id);

    }
}
