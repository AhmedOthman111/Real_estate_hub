using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Application.CQRS.AdPhotos.Commands
{
    public record UploadAdPhotoCommand(int AdId, Stream FileStream, string FileName, string AUOwnerId) : IRequest<string>;



}
