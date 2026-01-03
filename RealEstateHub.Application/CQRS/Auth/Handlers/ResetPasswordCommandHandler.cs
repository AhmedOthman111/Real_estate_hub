using MediatR;
using RealEstateHub.Application.CQRS.Auth.Commands;
using RealEstateHub.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Application.CQRS.Auth.Handlers
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand>
    {
        private readonly IAuthService _authService;
        public ResetPasswordCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task Handle (ResetPasswordCommand request , CancellationToken cancellationToken)
        {
            await _authService.ResetPasswordAsync(request.dto);
        }


    }
}
