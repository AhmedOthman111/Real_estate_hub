using MediatR;

namespace RealEstateHub.Application.CQRS.AdPhotos.Commands
{
    public record DeleteAdPhotoCommand(int PhotoId, string AUOwnerId) : IRequest;


}
