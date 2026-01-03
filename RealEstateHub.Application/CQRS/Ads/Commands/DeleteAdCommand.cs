using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Application.CQRS.Ads.Commands
{
    public record DeleteAdCommand(int id , string AUOwnerId) : IRequest;
}
