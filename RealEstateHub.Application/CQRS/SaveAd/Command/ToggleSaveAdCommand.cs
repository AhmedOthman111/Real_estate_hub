using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Application.CQRS.SaveAd.Command
{
    public record ToggleSaveAdCommand(int adId , string customerId) : IRequest;
    
    
}
