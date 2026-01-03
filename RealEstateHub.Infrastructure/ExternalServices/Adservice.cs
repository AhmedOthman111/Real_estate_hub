using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RealEstateHub.Application.Common;
using RealEstateHub.Application.CQRS.Ads.Dto;
using RealEstateHub.Application.Exceptions;
using RealEstateHub.Application.Interfaces;
using RealEstateHub.Domain.Entities;
using RealEstateHub.Domain.Enums;
using RealEstateHub.Infrastructure.Identity;

namespace RealEstateHub.Infrastructure.ExternalServices
{
    public class Adservice : IAdService
    {
        private readonly IUnitOfWork _uow;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;

        public Adservice(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, IEmailService emailService)
        {
            _uow = unitOfWork;
            _userManager = userManager;
            _emailService = emailService;
        }


        public async Task<AdResponseDto> CreateAdAsync(CreateAdDto dto, string AUOwnerId)
        {
            var owner = await _uow.Owner.GetByAppUserIdAsync(AUOwnerId);
            var AUowner = await _userManager.FindByIdAsync(owner.AppUserId);
            var category = await _uow.Category.GetByIdAsync(dto.CategoryId);


            var ad = new Ad
            {
                OwnerId = owner.Id,
                Title = dto.Title,
                Description = dto.Description,
                Price = dto.Price,
                AreaSize = dto.AreaSize,
                City = dto.City,
                Area = dto.Area,
                Address = dto.Address,
                Purpose = dto.Purpose,
                Priority = dto.Priority,
                DurationDays = dto.DurationDays,
                CategoryId = dto.CategoryId,
                Status = Domain.Enums.AdStatus.Pending,
                CreatedAt = DateTime.Now,
                ExpireAt = DateTime.Now.AddDays(dto.DurationDays),
            };


            await _uow.Ad.AddAsync(ad);
            await _uow.SaveChangesAsync();

            return MapToDto(ad, AUowner, category);
        }

        public async Task<AdResponseDto> UpdateAdAsync(UpdateAdDto dto, string AUOwnerId)
        {
            var ad = await _uow.Ad.GetByIdAsync(dto.Id);
            var AUowner = await _userManager.FindByIdAsync(AUOwnerId);
            var category = await _uow.Category.GetByIdAsync(dto.CategoryId);

            ad.Title = dto.Title;
            ad.Description = dto.Description;
            ad.Price = dto.Price;
            ad.AreaSize = dto.AreaSize;
            ad.City = dto.City;
            ad.Area = dto.Area;
            ad.Address = dto.Address;
            ad.Purpose = dto.Purpose;
            ad.CategoryId = dto.CategoryId;

            _uow.Ad.Update(ad);
            await _uow.SaveChangesAsync();

            return MapToDto(ad, AUowner, category);
        }

        public async Task DeleteAdAsync(int id, string AUOwnerId)
        {
            var ad = await _uow.Ad.GetByIdAsync(id);
            var owner = await _uow.Owner.GetByAppUserIdAsync(AUOwnerId);

            if (ad.OwnerId != owner.Id)
                throw new BusinessException("You are not authorized to delete this ad.");

            ad.Status = Domain.Enums.AdStatus.Deleted;
            _uow.Ad.Update(ad);
            await _uow.SaveChangesAsync();
        }

        public async Task ChangeStatusAsync(int id, string AUOwnerId)
        {
            var ad = await _uow.Ad.GetByIdAsync(id);

            var owner = await _uow.Owner.GetByAppUserIdAsync(AUOwnerId);
            if (ad.OwnerId != owner.Id)
                throw new BusinessException("You are not authorized to delete this ad.");

            if (ad.Purpose == AdPurpose.Sale) ad.Status = AdStatus.Sold;
            else if (ad.Purpose == AdPurpose.Rent) ad.Status = AdStatus.Rented;
            else throw new BusinessException(" Changing the ad status is not allowed.");

            _uow.Ad.Update(ad);
            await _uow.SaveChangesAsync();

        }

        public async Task RenewAdAsync(int id, int durationDays, string AUOwnerId)
        {
            var ad = await _uow.Ad.GetByIdAsync(id);

            var owner = await _uow.Owner.GetByAppUserIdAsync(AUOwnerId);
            if (ad.OwnerId != owner.Id)
                throw new BusinessException("You are not authorized to renew this ad.");

            ad.DurationDays = durationDays;
            ad.ExpireAt = DateTime.UtcNow.AddDays(durationDays);
            ad.Status = AdStatus.Active;

            _uow.Ad.Update(ad);
            await _uow.SaveChangesAsync();
        }

        public async Task<List<AdResponseDto>> GetMyAdsAsync(string AUOwnerId)
        {
            var owner = await _uow.Owner.GetByAppUserIdAsync(AUOwnerId);

            var AUOwner = await _userManager.FindByIdAsync(AUOwnerId);

            var OwnerAds = await _uow.Ad.GetAllOwnerAds(owner.Id);

            return OwnerAds.Select(a => MapToDto(a, AUOwner, a.Category)).ToList();

        }

        public async Task<AdResponseDto> GetAdByIdAsync(int id)
        {
            var ad = await _uow.Ad.GetAdWithPhotos(id);

            var AppUserOwner = await _userManager.FindByIdAsync(ad.Owner.AppUserId);

            return MapToDto(ad, AppUserOwner, ad.Category);
        }

        public async Task<PaginatedList<AdResponseDto>> GetAdsAsync(AdFilterDto filter)
        {
            var paginatedAds = await _uow.Ad.GetFilteredAdsAsync(filter);

            var ownerUserIds = paginatedAds.Items
                .Select(a => a.Owner.AppUserId)
                .Distinct()
                .ToList();

            var ownerUsers = await _userManager.Users
                .Where(u => ownerUserIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id);


            var dtos = new List<AdResponseDto>();

            foreach (var ad in paginatedAds.Items)
            {
                var ownerUser = ownerUsers[ad.Owner.AppUserId];
                dtos.Add(MapToDto(ad, ownerUser, ad.Category));
            }

            return new PaginatedList<AdResponseDto>(dtos, paginatedAds.TotalCount, paginatedAds.PageIndex, paginatedAds.PageSize);

        }

        public async Task<IReadOnlyList<AdResponseDto>> GetAllPendingAds()
        {
            var PendingAds = await _uow.Ad.GetPendingAdsAsync();

            var ownerUserIds = PendingAds.Select(a => a.Owner.AppUserId).Distinct().ToList();

            var AppUserOwner = await _userManager.Users.Where(a => ownerUserIds.Contains(a.Id)).ToDictionaryAsync(u => u.Id);

            var dtos = new List<AdResponseDto>();

            foreach (var ad in PendingAds)
            {
                var ownerUser = AppUserOwner[ad.Owner.AppUserId];
                dtos.Add(MapToDto(ad, ownerUser, ad.Category));
            }

            return dtos;

        }

        public async Task ApproveAd(int AdId)
        {
            var ad = await _uow.Ad.GetAdWithOwner(AdId);
            if (ad.Status != AdStatus.Pending) throw new BusinessException("Ad statue is Not Pending to be Approved");

            ad.Status = AdStatus.Active;

            _uow.Ad.Update(ad);
            await _uow.SaveChangesAsync();

            var AUOwner = await _userManager.FindByIdAsync(ad.Owner.AppUserId);
            string fullname = $"{AUOwner.FirstName} {AUOwner.LastName}";


            BackgroundJob.Enqueue(() => _emailService.SendAdApprovedEmailAsync(AUOwner.Email, fullname, ad.Title));
        }

        public async Task RejectAd(int AdId, string? rejectionReason = null)
        {
            var ad = await _uow.Ad.GetAdWithOwner(AdId);
            if (ad.Status != AdStatus.Pending) throw new BusinessException("Ad statue is Not Pending to be Approved");

            ad.Status = AdStatus.Rejected;

            _uow.Ad.Update(ad);
            await _uow.SaveChangesAsync();

            var AUOwner = await _userManager.FindByIdAsync(ad.Owner.AppUserId);
            string fullname = $"{AUOwner.FirstName} {AUOwner.LastName}";

            if (!string.IsNullOrWhiteSpace(rejectionReason))
            {

                BackgroundJob.Enqueue(() => _emailService.SendAdRejectedEmailAsync(AUOwner.Email, fullname, ad.Title, rejectionReason));
            }
            else
            {

                BackgroundJob.Enqueue(() => _emailService.SendAdRejectedWithoutReasonEmailAsync(AUOwner.Email, fullname, ad.Title));
            }
        }



        private AdResponseDto MapToDto(Ad ad, AppUser ownerappuser, Category category)
        {

            var PhotoUrls = ad.Photos?
                            .OrderByDescending(p => p.IsMain)
                            .ThenBy(p => p.Id)
                            .Select(p => p.ImageUrl)
                            .ToList()
                            ?? new List<string>();

            return new AdResponseDto
            {
                Id = ad.Id,
                Title = ad.Title,
                Description = ad.Description,
                Price = ad.Price,
                AreaSize = ad.AreaSize,
                City = ad.City,
                Area = ad.Area,
                Address = ad.Address,
                Purpose = ad.Purpose.ToString(),
                Priority = ad.Priority.ToString(),
                Status = ad.Status.ToString(),
                CreatedAt = ad.CreatedAt,
                ExpireAt = ad.ExpireAt,
                OwnerId = ad.OwnerId,
                OwnerName = $"{ownerappuser.FirstName} {ownerappuser.LastName}",
                CategoryName = category.Name,
                PhotoUrls = PhotoUrls
            };
        }
    }
}
