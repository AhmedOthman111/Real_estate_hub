using MediatR;
using RealEstateHub.Application.CQRS.Profile.Dto;
using RealEstateHub.Application.CQRS.Profile.Queries;
using RealEstateHub.Application.Interfaces;

namespace RealEstateHub.Application.CQRS.Profile.Handler
{
    public class GetOwnerProfileQueryHandler : IRequestHandler<GetOwnerProfileQuery, OwnerProfileDto>
    {
        private readonly IProfileService _profileService;
        private readonly ICacheService _cacheService;
        public GetOwnerProfileQueryHandler(IProfileService profileService, ICacheService cacheService)
        {
            _cacheService = cacheService;
            _profileService = profileService;
        }

        public async Task<OwnerProfileDto> Handle(GetOwnerProfileQuery request, CancellationToken cancellationToken)
        {
            string cahcekey = $"OwnerPublicProfile:{request.ownerId}";

            var cached = await _cacheService.GetAsync<OwnerProfileDto>(cahcekey);
            if (cached != null) return cached;

            var profile = await _profileService.GetOwnerProfileAsync(request.ownerId);

            await _cacheService.SetAsync(cahcekey, profile, TimeSpan.FromMinutes(5), null, cancellationToken);

            return profile;
        }
    }
}
