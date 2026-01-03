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
    public class ChangeAdStatusCommandHandler : IRequestHandler<ChangeAdStatusCommand>
    {
        private readonly IAdService _adService;
        private readonly ICacheService _cacheService;
        public ChangeAdStatusCommandHandler(IAdService adService , ICacheService cacheService )
        {
            _adService = adService;
            _cacheService = cacheService;
        }

        public async Task Handle(ChangeAdStatusCommand request , CancellationToken cancellationToken)
        {
            await _adService.ChangeStatusAsync(request.id, request.AUOwnerId);
            await _cacheService.RemoveAsync($"ad:{request.id}", cancellationToken);
        }
    }
}
