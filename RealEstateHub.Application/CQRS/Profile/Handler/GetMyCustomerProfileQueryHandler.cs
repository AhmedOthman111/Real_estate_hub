using MediatR;
using RealEstateHub.Application.CQRS.Profile.Dto;
using RealEstateHub.Application.CQRS.Profile.Queries;
using RealEstateHub.Application.Interfaces;

namespace RealEstateHub.Application.CQRS.Profile.Handler
{
    public class GetMyCustomerProfileQueryHandler : IRequestHandler<GetMyCustomerProfileQuery, CustomerProfileDto>
    {
        private readonly IProfileService _profileService;
        private readonly ICacheService _cacheService;
        public GetMyCustomerProfileQueryHandler(ICacheService cacheService, IProfileService profileService)
        {

            _cacheService = cacheService;
            _profileService = profileService;
        }

        public async Task<CustomerProfileDto> Handle(GetMyCustomerProfileQuery request, CancellationToken cancellationToken)
        {
            var cachekey = $"CustomerProfileId:{request.customerId}";

            var cached = await _cacheService.GetAsync<CustomerProfileDto>(cachekey);

            if (cached != null) return cached;

            var profile = await _profileService.GetmyCustomerProfileAsync(request.customerId);

            await _cacheService.SetAsync(cachekey, profile, TimeSpan.FromMinutes(5), null, cancellationToken);
            return profile;
        }


    }
}
