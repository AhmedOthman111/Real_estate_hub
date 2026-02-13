using MediatR;
using RealEstateHub.Application.CQRS.SaveAd.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Application.CQRS.SaveAd.Queries
{
    public record GetMySavedQuery(string customerId) : IRequest<List<SaveAdResponseDto>>;
    
    
}
