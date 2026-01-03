using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using RealEstateHub.Application.Interfaces;
using RealEstateHub.Application.Common;

namespace RealEstateHub.Infrastructure.ExternalServices
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        public TokenService(IConfiguration config)
        {
            _config = config;
        }
        public Task<string> CreateTokenAsync(TokenUserDTO user)
        {
            var AuthClaims = new List<Claim>
            {

                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("fullName", user.FullName)
            };
            
            AuthClaims.AddRange(user.Roles.Select(
                                                  role => new Claim(ClaimTypes.Role, role) ));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            
            var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            int expiryMinutes = int.Parse(_config["Jwt:DurationInMinutes"]!);

            
            var token = new JwtSecurityToken(
              issuer: _config["Jwt:Issuer"],
              audience: _config["Jwt:Audience"],
              claims: AuthClaims,
              expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
              signingCredentials: creds
              );

            return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));    

        }
    }
}
