using RealEstateHub.Application.CQRS.SaveAd.Dto;
using RealEstateHub.Application.Exceptions;
using RealEstateHub.Application.Interfaces;
using RealEstateHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Infrastructure.ExternalServices
{
    public class SaveAdService : ISaveAdService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SaveAdService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task ToggleSaveAsync(int adId , string customerId)
        {
            if (!await _unitOfWork.Ad.AnyAsync(a => a.Id == adId))
                throw new NotFoundException("Ad not found");

            var existing =  (await _unitOfWork.SaveAdRepo.FindAsync(s=> s.AdId == adId && s.CustomerId == customerId)).FirstOrDefault();

            if (existing != null)
            {
                _unitOfWork.SaveAdRepo.Delete(existing);
            }
            else
            {
                var newsave = new Favorite
                {
                    AdId = adId,
                    CustomerId = customerId,
                    CreatedAt = DateTime.Now
                };
                
                await _unitOfWork.SaveAdRepo.AddAsync(newsave);
            }

            await _unitOfWork.SaveChangesAsync();
        }

      public async Task<List<SaveAdResponseDto>> GetMySavedAsync(string customerId)
      {
            return await _unitOfWork.SaveAdRepo.GetAllSavedAdsbyCustomerId(customerId);
      }




    }
}
