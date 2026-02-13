using MediatR;
using RealEstateHub.Application.CQRS.Rating.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Application.CQRS.Rating.Command
{
    public record CreateRatingCommand (CreateRatingDto dto , string customerid) :IRequest<RatingResponseDto>;
    
    
}
