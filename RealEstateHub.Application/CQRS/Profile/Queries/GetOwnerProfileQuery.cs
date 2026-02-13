using MediatR;
using RealEstateHub.Application.CQRS.Profile.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Application.CQRS.Profile.Queries
{
    public record GetOwnerProfileQuery(int ownerId) : IRequest<OwnerProfileDto>;
    
}
