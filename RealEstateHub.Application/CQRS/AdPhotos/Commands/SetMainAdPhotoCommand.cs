using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Application.CQRS.AdPhotos.Commands
{
    public record SetMainAdPhotoCommand(int PhotoId, string AUOwnerId) : IRequest;
    
    
}
