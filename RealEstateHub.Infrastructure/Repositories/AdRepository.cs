using Microsoft.EntityFrameworkCore;
using RealEstateHub.Application.Common;
using RealEstateHub.Application.CQRS.Ads.Dto;
using RealEstateHub.Application.Exceptions;
using RealEstateHub.Application.Interfaces.IRep;
using RealEstateHub.Domain.Entities;
using RealEstateHub.Domain.Enums;
using RealEstateHub.Infrastructure.Data;
using RealEstateHub.Infrastructure.ExternalServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Infrastructure.Repositories
{
    public class AdRepository : GenericRepository<Ad>, IAdRepository
    {
        public AdRepository(ApplicationDbContext context) : base(context) {}

        public async Task<IEnumerable<Ad>> GetActiveAdsAsync()
        {
            var ads = await _dbSet.Where(x => x.Status == AdStatus.Active).ToListAsync();

            if (ads.Count() == 0) throw new NotFoundException("NO Active Ads Found");
            return ads;
        }
        public async Task<Ad?> GetAdWithPhotos(int id)
        {
             var ad = await _dbSet
                .AsNoTracking()
                .Include(x => x.Photos)
                .Include(x => x.Category)
                .Include(x => x.Owner)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (ad == null) throw new NotFoundException("ad", id);
            return ad;
        }

        public async Task<IReadOnlyList<Ad>> GetAllOwnerAds(int ownerId)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(x => x.OwnerId == ownerId && x.Status != AdStatus.Deleted)
                .Include(x => x.Photos).Include(x => x.Category)
                .ToListAsync();
        }

        public async Task<PaginatedList<Ad>> GetFilteredAdsAsync(AdFilterDto filter)
        {
            var query = _dbSet.AsQueryable().AsNoTracking();
            
            query = query.Where(x => x.Status == AdStatus.Active);

            if(!string.IsNullOrEmpty(filter.Search))
            {
                var search = filter.Search.Trim();
               
                query = query.Where(x =>
                    x.Title.Contains(search) ||
                    x.Description.Contains(search));
            }

            if (filter.Purpose.HasValue)
            {
                query = query.Where(x => x.Purpose == filter.Purpose.Value);
            }
          
            if (!string.IsNullOrEmpty(filter.City))
            {
                query = query.Where(x => x.City == filter.City);
            }

            if(filter.MinPrice.HasValue) 
            {
                query = query.Where(x => x.Price >= filter.MinPrice.Value);
            }
           
            if (filter.MaxPrice.HasValue)
            {
                query = query.Where(x => x.Price <= filter.MaxPrice.Value);
            }

            if (filter.MinArea.HasValue)
            {
                query = query.Where(x => x.AreaSize >= filter.MinArea.Value);
            }

            if (filter.MaxArea.HasValue)
            {
                query = query.Where(x => x.AreaSize <= filter.MaxArea.Value);
            }

            if (filter.CategoryId.HasValue)
            {
                query = query.Where(x => x.CategoryId == filter.CategoryId.Value);
            }


            query = query
                .Include(x => x.Photos)
                .Include(x => x.Owner)
                .Include(x => x.Category)
                .OrderByDescending(x => x.Priority)
                .ThenByDescending(x => x.CreatedAt);


            return await PaginationExtensions.ToPaginatedListAsync(query, filter.Page, filter.PageSize);

        }

        public async Task<IReadOnlyList<Ad>> GetPendingAdsAsync()
        {
            return await _dbSet
                .Where(x => x.Status == AdStatus.Pending)
                .Include(x => x.Owner)
                .Include(x => x.Category)
                .Include(x => x.Photos)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Ad?> GetAdWithOwner(int id)
        {
            var ad = await _dbSet
               .AsNoTracking()
               .Include(x => x.Owner)
               .FirstOrDefaultAsync(x => x.Id == id);
            if (ad == null) throw new NotFoundException("ad", id);
            return ad;
        }




    }
}
