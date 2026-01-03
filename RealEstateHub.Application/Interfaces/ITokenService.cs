using RealEstateHub.Application.Common;

namespace RealEstateHub.Application.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateTokenAsync(TokenUserDTO user);

    }
}