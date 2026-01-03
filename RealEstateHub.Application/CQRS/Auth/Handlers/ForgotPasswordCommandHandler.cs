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
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand>
    {

        private readonly IAuthService _authService;
        public ForgotPasswordCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task Handle (ForgotPasswordCommand request , CancellationToken cancellationToken)
        {
            await _authService.ForgotPasswordAsync(request.email);
        }
    }
}
