using RealEstateHub.Application.CQRS.Auth.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterCustomerAsync(RegisterCustomerDto dto);
        Task<AuthResponseDto> RegisterOwnerAsync(RegisterOwnerDto dto);
        Task<AuthResponseDto> ConfirmEmailAsync(ConfirmEmailDto dto);
        Task<AuthResponseDto> Login(LoginDto dto);
        Task ForgotPasswordAsync(string email);
        Task ResetPasswordAsync(ResetPasswordDto dto);
        Task<AuthResponseDto> ExternalLoginGoogleAsync(GoogleLoginDto dto);

    }
}
