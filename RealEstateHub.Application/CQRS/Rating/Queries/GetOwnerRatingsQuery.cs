using MediatR;
using RealEstateHub.Application.CQRS.Rating.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Application.CQRS.Rating.Queries
{
    public record GetOwnerRatingsQuery(int ownerid) : IRequest<List<RatingResponseDto>>;
    
}
