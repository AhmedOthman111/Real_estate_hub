using RealEstateHub.Application.CQRS.Rating.Dto;

namespace RealEstateHub.Application.Interfaces
{
    public interface IRatingService
    {
        Task<RatingResponseDto> AddRatingAsync(CreateRatingDto dto, string customerId);
        Task<List<RatingResponseDto>> GetRatingsByOwnerIdAsync(int ownerId);


    }
}
