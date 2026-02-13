using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RealEstateHub.Application.CQRS.Rating.Dto;
using RealEstateHub.Application.Exceptions;
using RealEstateHub.Application.Interfaces;
using RealEstateHub.Domain.Entities;
using RealEstateHub.Infrastructure.Identity;

namespace RealEstateHub.Infrastructure.ExternalServices
{
    public class RatingService : IRatingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly ICacheService _cacheService;
        public RatingService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _cacheService = cacheService;
        }

        public async Task<RatingResponseDto> AddRatingAsync(CreateRatingDto dto, string customerId)
        {
            if (dto.Stars < 1 || dto.Stars > 5)
                throw new BusinessException("Stars must be between 1 and 5.");


            var owner = await _unitOfWork.Owner.GetByIdAsync(dto.OwnerId);


            var existrating = await _unitOfWork.RatingRepo
                            .AnyAsync(r => r.OwnerId == dto.OwnerId && r.CustomerID == customerId);

            if (existrating)
                throw new BusinessException("You have already rated this owner.");

            var rating = new Rating
            {
                OwnerId = dto.OwnerId,
                Stars = dto.Stars,
                Review = dto.Review,
                CreatedAt = DateTime.UtcNow,
                CustomerID = customerId
            };

            await _unitOfWork.RatingRepo.AddAsync(rating);

            await _unitOfWork.SaveChangesAsync();

            owner.AverageRating = await CalculateOwnerAverageRating(dto.OwnerId);

            _unitOfWork.Owner.Update(owner);

            await _unitOfWork.SaveChangesAsync();

            string cahcekey1 = $"OwnerPublicProfile:{owner.Id}";
            string cahcekey2 = $"OwnerMyProfile:{owner.AppUserId}";

            await _cacheService.RemoveAsync(cahcekey1);
            await _cacheService.RemoveAsync(cahcekey2);


            var user = await _userManager.FindByIdAsync(customerId);

            return new RatingResponseDto
            {
                id = rating.Id,
                OwnerId = rating.OwnerId,
                Stars = rating.Stars,
                Review = rating.Review,
                CreatedAt = rating.CreatedAt,
                AuthorName = user != null ? $"{user.FirstName} {user.LastName}" : "Unknown"
            };

        }

        public async Task<List<RatingResponseDto>> GetRatingsByOwnerIdAsync(int ownerId)
        {
            var owner = await _unitOfWork.Owner.GetByIdAsync(ownerId);

            var ratings = await _unitOfWork.RatingRepo.FindAsync(r => r.OwnerId == owner.Id);

            var response = new List<RatingResponseDto>();

            if (!ratings.Any()) return response;

            var userIds = ratings
                                .Select(r => r.CustomerID)
                                .Distinct()
                                .ToList();

            var users = await _userManager.Users
                .Where(u => userIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id);


            return ratings.Select(r =>
            {
                users.TryGetValue(r.CustomerID, out var user);

                return new RatingResponseDto
                {
                    id = r.Id,
                    OwnerId = r.OwnerId,
                    Stars = r.Stars,
                    Review = r.Review,
                    CreatedAt = r.CreatedAt,
                    AuthorName = user != null ? $"{user.FirstName} {user.LastName}" : "Unknown"
                };
            }).ToList();
        }


        private async Task<double> CalculateOwnerAverageRating(int ownerId)
        {
            var score = await _unitOfWork.RatingRepo .FindAsync(r => r.OwnerId == ownerId);

            return score.Any() ? score.Average(r => r.Stars) : 0;
        }


    }
}
