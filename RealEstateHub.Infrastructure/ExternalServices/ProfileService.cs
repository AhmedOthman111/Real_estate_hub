using Azure.Core;
using Microsoft.AspNetCore.Identity;
using RealEstateHub.Application.CQRS.Profile.Dto;
using RealEstateHub.Application.Exceptions;
using RealEstateHub.Application.Interfaces;
using RealEstateHub.Infrastructure.Identity;

namespace RealEstateHub.Infrastructure.ExternalServices
{
    public class ProfileService : IProfileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly IRatingService _ratingService;
        private readonly ICacheService _cacheService;
        public ProfileService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, IRatingService ratingService , ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _ratingService = ratingService;
            _cacheService = cacheService;
        }

        public async Task<CustomerProfileDto> GetmyCustomerProfileAsync(string customerId)
        {
            var customer = await _userManager.FindByIdAsync(customerId);
            if (customer == null) throw new NotFoundException("customer", customerId);

            return new CustomerProfileDto()
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                NationalId = customer.NationalId,
                PhoneNumber = customer.PhoneNumber != null ? customer.PhoneNumber : "N/A",
                UserName = customer.UserName != null ? customer.UserName : "N/A"
            };

        }

        public async Task UpdateCustomerProfileAsync(string customerId, UpdateCustomerProfileDto dto)
        {
            var customer = await _userManager.FindByIdAsync(customerId);
            if (customer == null) throw new NotFoundException("User", customerId);

            customer.FirstName = dto.FirstName;
            customer.LastName = dto.LastName;
            customer.PhoneNumber = dto.PhoneNumber;
            customer.NationalId = dto.NationalId;

            var result = await _userManager.UpdateAsync(customer);
            if (!result.Succeeded)
            {
                throw new BusinessException($"Failed to update profile: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

        }

        public async Task<OwnerProfileDto> GetOwnerProfileAsync(int ownerId)
        {
            var owner = await _unitOfWork.Owner.GetByIdAsync(ownerId);

            var ownerAppUser = await _userManager.FindByIdAsync(owner.AppUserId);

            var ratings = await _ratingService.GetRatingsByOwnerIdAsync(ownerId);

            return new OwnerProfileDto
            {
                FirstName = ownerAppUser.FirstName,
                LastName = ownerAppUser.LastName,
                CompanyName = owner.CompanyName,
                Bio = owner.Bio,
                Address = owner.Address,
                WhatsappNumber = owner.WhatsappNumber,
                AverageRating = owner.AverageRating,
                Ratings = ratings,
            };
        }
        public async Task<OwnerProfileDto> GetMyOwnerProfileAsync(string ownerId)
        {
            var owner = await _unitOfWork.Owner.GetByAppUserIdAsync(ownerId);

            var ownerAppUser = await _userManager.FindByIdAsync(ownerId);

            var ratings = await _ratingService.GetRatingsByOwnerIdAsync(owner.Id);

            return new OwnerProfileDto
            {
                Mail = ownerAppUser.Email,
                UserName = ownerAppUser.UserName,
                FirstName = ownerAppUser.FirstName,
                LastName = ownerAppUser.LastName,
                CompanyName = owner.CompanyName,
                Bio = owner.Bio,
                Address = owner.Address,
                WhatsappNumber = owner.WhatsappNumber,
                AverageRating = owner.AverageRating,
                Ratings = ratings,
            };
        }

        public async Task UpdateOwnerProfileAsync(string ownerId, UpdateOwnerProfileDto dto)
        {
            var appuserowner = await _userManager.FindByIdAsync(ownerId);

            var owner = await _unitOfWork.Owner.GetByAppUserIdAsync(ownerId);

            appuserowner.FirstName = dto.FirstName;
            appuserowner.LastName = dto.LastName;

            await _userManager.UpdateAsync(appuserowner);

            owner.Address = dto.Address;
            owner.WhatsappNumber = dto.WhatsappNumber;
            owner.CompanyName = dto.CompanyName;
            owner.Bio = dto.Bio;

            _unitOfWork.Owner.Update(owner);

            await _unitOfWork.SaveChangesAsync();

            string cahcekey1 = $"OwnerPublicProfile:{owner.Id}";
            await _cacheService.RemoveAsync(cahcekey1);

            string cahcekey2 = $"OwnerMyProfile:{ownerId}";
            await _cacheService.RemoveAsync(cahcekey2);

        }




    }
}
