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
    public class RegisterOwnerCommandHandler : IRequestHandler< RegisterOwnerCommand , AuthResponseDto>
    {
        private readonly IAuthService _authService;
        public RegisterOwnerCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<AuthResponseDto> Handle (RegisterOwnerCommand request , CancellationToken cancellationToken)
        {
            return await _authService.RegisterOwnerAsync(request.dto);
        }

    }
}
