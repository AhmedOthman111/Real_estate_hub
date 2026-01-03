using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Application.CQRS.Ads.Commands
{
    public record RejectAdCommand(int id , string? rejectionReason) : IRequest;
}
