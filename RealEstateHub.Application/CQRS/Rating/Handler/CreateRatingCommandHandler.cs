using MediatR;
using RealEstateHub.Application.CQRS.Ads.Commands;
using RealEstateHub.Application.CQRS.Rating.Command;
using RealEstateHub.Application.CQRS.Rating.Dto;
using RealEstateHub.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Application.CQRS.Rating.Handler
{
    public class CreateRatingCommandHandler : IRequestHandler<CreateRatingCommand , RatingResponseDto>
    {
        private readonly IRatingService _ratingService;
        public CreateRatingCommandHandler(IRatingService ratingService) 
        {
            _ratingService = ratingService;
        }

        public async Task<RatingResponseDto> Handle(CreateRatingCommand request, CancellationToken cancellationToken)
        {
            return await _ratingService.AddRatingAsync(request.dto, request.customerid);
        }
    }
}
