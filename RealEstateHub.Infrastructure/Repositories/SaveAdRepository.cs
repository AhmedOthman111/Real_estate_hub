using Microsoft.EntityFrameworkCore;
using RealEstateHub.Application.CQRS.SaveAd.Dto;
using RealEstateHub.Application.Interfaces.IRep;
using RealEstateHub.Domain.Entities;
using RealEstateHub.Infrastructure.Data;

namespace RealEstateHub.Infrastructure.Repositories
{
    public class SaveAdRepository : GenericRepository<Favorite>, ISaveAdRepository
    {

        public SaveAdRepository(ApplicationDbContext dbContext) : base(dbContext)
        { }


        public async Task<List<SaveAdResponseDto>> GetAllSavedAdsbyCustomerId(string customerId)
        {
            return await _dbSet
                .Where(s => s.CustomerId == customerId)
                .OrderByDescending(s => s.CreatedAt)
                .AsNoTracking()
                .Select(s => new SaveAdResponseDto
                {
                    Id = s.Id,
                    AdId = s.AdId,
                    AdPurpose = s.Ad.Purpose.ToString(),
                    AdTitle = s.Ad.Title,
                    CategoryName = s.Ad.Category.Name,
                    CreatedAt = s.CreatedAt
                })
                .ToListAsync();
        }

    }
}
