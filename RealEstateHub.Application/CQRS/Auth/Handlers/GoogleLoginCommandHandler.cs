using MediatR;
using RealEstateHub.Application.CQRS.Auth.Commands;
using RealEstateHub.Application.CQRS.Auth.Dto;
using RealEstateHub.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Application.CQRS.Auth.Handlers
{
    public class GoogleLoginCommandHandler : IRequestHandler<GoogleLoginCommand, AuthResponseDto>
    {
        private readonly IAuthService _authService;

        public GoogleLoginCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<AuthResponseDto> Handle(GoogleLoginCommand request, CancellationToken cancellationToken)
        {
            return await _authService.ExternalLoginGoogleAsync(request.Dto);
        }
    }
}
