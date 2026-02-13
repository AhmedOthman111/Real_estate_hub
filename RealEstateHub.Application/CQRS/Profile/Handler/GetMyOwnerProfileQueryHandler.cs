using MediatR;
using RealEstateHub.Application.CQRS.Profile.Dto;
using RealEstateHub.Application.CQRS.Profile.Queries;
using RealEstateHub.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Application.CQRS.Profile.Handler
{
    public class GetMyOwnerProfileQueryHandler : IRequestHandler<GetMyOwnerProfileQuery,OwnerProfileDto>
    {

        private readonly IProfileService _profileService;
        private readonly ICacheService _cacheService;
        public GetMyOwnerProfileQueryHandler(IProfileService profileService , ICacheService cacheService) 
        {
            _profileService = profileService;
            _cacheService = cacheService;
        }

        public async Task<OwnerProfileDto> Handle (GetMyOwnerProfileQuery request ,CancellationToken cancellationToken)
        {
            string cahcekey = $"OwnerMyProfile:{request.ownerId}";
            var cached = await _cacheService.GetAsync<OwnerProfileDto>(cahcekey);
            if (cached != null) return cached;

            var profile = await _profileService.GetMyOwnerProfileAsync(request.ownerId);

            await _cacheService.SetAsync(cahcekey, profile, TimeSpan.FromMinutes(5), null, cancellationToken);

            return profile;

        }
    }
}
