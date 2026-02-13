using RealEstateHub.Application.CQRS.SaveAd.Dto;
using RealEstateHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Application.Interfaces.IRep
{
    public interface ISaveAdRepository : IGenericRepository<Favorite>
    {
        Task<List<SaveAdResponseDto>> GetAllSavedAdsbyCustomerId(string customerId);
    }
}
