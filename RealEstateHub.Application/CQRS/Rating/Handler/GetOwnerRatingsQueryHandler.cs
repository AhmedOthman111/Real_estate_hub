using MediatR;
using RealEstateHub.Application.CQRS.Rating.Dto;
using RealEstateHub.Application.CQRS.Rating.Queries;
using RealEstateHub.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Application.CQRS.Rating.Handler
{
    public class GetOwnerRatingsQueryHandler : IRequestHandler<GetOwnerRatingsQuery,List<RatingResponseDto>>
    {
        private IRatingService _ratingService;
        public GetOwnerRatingsQueryHandler(IRatingService ratingService) 
        {
            _ratingService = ratingService;
        }

        public async Task<List<RatingResponseDto>> Handle (GetOwnerRatingsQuery request , CancellationToken cancellationToken)
        {
            return await _ratingService.GetRatingsByOwnerIdAsync(request.ownerid);
        }
    }
}
