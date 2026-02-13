using MediatR;
using RealEstateHub.Application.CQRS.Profile.Command;
using RealEstateHub.Application.Interfaces;

namespace RealEstateHub.Application.CQRS.Profile.Handler
{
    public class UpdateOwnerProfileCommandHandler : IRequestHandler<UpdateOwnerProfileCommand>
    {
        private readonly IProfileService _profileService;
        public UpdateOwnerProfileCommandHandler(IProfileService profileService)
        {
            _profileService = profileService;
        }
        public async Task Handle(UpdateOwnerProfileCommand request, CancellationToken cancellationToken)
        {
            await _profileService.UpdateOwnerProfileAsync(request.ownerId, request.dto);
        }
    }
}
