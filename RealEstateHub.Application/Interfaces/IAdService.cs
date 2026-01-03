using RealEstateHub.Application.Common;
using RealEstateHub.Application.CQRS.Ads.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Application.Interfaces
{
    public interface IAdService
    {
        Task<AdResponseDto> CreateAdAsync(CreateAdDto dto, string AUOwnerId);
        Task<AdResponseDto> UpdateAdAsync(UpdateAdDto dto, string AUOwnerId);
        Task DeleteAdAsync(int id, string AUOwnerId);
        Task ChangeStatusAsync(int id, string AUOwnerId);
        Task RenewAdAsync(int id, int durationDays, string AUOwnerId);
        Task<List<AdResponseDto>> GetMyAdsAsync(string AUOwnerId);
        Task<AdResponseDto> GetAdByIdAsync(int id);
        Task<PaginatedList<AdResponseDto>> GetAdsAsync(AdFilterDto filter);
        Task<IReadOnlyList<AdResponseDto>> GetAllPendingAds();
        Task ApproveAd(int AdId);
        Task RejectAd(int AdId, string? rejectionReason = null);


    }
}
