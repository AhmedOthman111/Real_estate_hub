using RealEstateHub.Application.CQRS.Profile.Dto;

namespace RealEstateHub.Application.Interfaces
{
    public interface IProfileService
    {
        Task<CustomerProfileDto> GetmyCustomerProfileAsync(string customerId);
        Task UpdateCustomerProfileAsync(string customerId, UpdateCustomerProfileDto dto);
        Task<OwnerProfileDto> GetOwnerProfileAsync(int ownerId);
        Task<OwnerProfileDto> GetMyOwnerProfileAsync(string ownerId);
        Task UpdateOwnerProfileAsync(string ownerId, UpdateOwnerProfileDto dto);



    }
}
