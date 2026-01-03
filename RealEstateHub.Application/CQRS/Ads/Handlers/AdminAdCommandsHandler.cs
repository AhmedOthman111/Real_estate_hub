using MediatR;
using RealEstateHub.Application.CQRS.Ads.Commands;
using RealEstateHub.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Application.CQRS.Ads.Handlers
{
    public class AdminAdCommandsHandler : IRequestHandler<ApproveAdCommand>  , IRequestHandler<RejectAdCommand>
    {
        private readonly IAdService _adService;
        public AdminAdCommandsHandler(IAdService adService)
        {
            _adService = adService;
        }

        public async Task Handle(ApproveAdCommand request, CancellationToken cancellationToken)
        {
            await _adService.ApproveAd(request.id);
        }

        public async Task Handle(RejectAdCommand request, CancellationToken cancellationToken)
        {
            await _adService.RejectAd(request.id , request.rejectionReason);
        }

    }
}
