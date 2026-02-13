using MediatR;
using RealEstateHub.Application.CQRS.Profile.Command;
using RealEstateHub.Application.Interfaces;

namespace RealEstateHub.Application.CQRS.Profile.Handler
{
    public class UpdateCustomerProfileCommandHandler : IRequestHandler<UpdateCustomerProfileCommand>
    {
        private readonly IProfileService _profileService;
        private readonly ICacheService _cacheService;
        public UpdateCustomerProfileCommandHandler(IProfileService profileService , ICacheService cacheService)
        {
            _profileService = profileService;
            _cacheService = cacheService;
        }

        public async Task Handle(UpdateCustomerProfileCommand request, CancellationToken cancellationToken)
        {
            await _profileService.UpdateCustomerProfileAsync(request.customerId, request.dto);

            var cachekey = $"CustomerProfileId:{request.customerId}";
            await _cacheService.RemoveAsync(cachekey);
        }
    }
}
