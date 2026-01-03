using MediatR;
using RealEstateHub.Application.CQRS.Auth.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Application.CQRS.Auth.Commands
{
    public record LoginCommand (LoginDto dto) : IRequest<AuthResponseDto> { }
    
}
