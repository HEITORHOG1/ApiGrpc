using ApiGrpc.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiGrpc.Infrastructure.Services
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;
        private readonly TokenValidationParameters _validationParameters;

        public (string accessToken, string refreshToken) GenerateJwtToken(ApplicationUser user)
        {
            return GenerateTokens(user);
        }

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
            _validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
            };
        }

        public (string accessToken, string refreshToken) GenerateTokens(ApplicationUser user)
        {
            var accessToken = GenerateAccessToken(user);
            var refreshToken = GenerateRefreshToken(user);
            return (accessToken, refreshToken);
        }

        public string GenerateAccessToken(ApplicationUser user)
        {
            var claims = new List<Claim> {
           new(JwtRegisteredClaimNames.Sub, user.Id),
           new(JwtRegisteredClaimNames.Email, user.Email),
           new(ClaimTypes.Name, user.UserName),
           new("firstName", user.FirstName),
           new("lastName", user.LastName)
       };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken(ApplicationUser user)
        {
            var claims = new List<Claim> {
           new(JwtRegisteredClaimNames.Sub, user.Id),
           new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
       };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:RefreshKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public ClaimsPrincipal ValidateRefreshToken(string refreshToken)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(refreshToken, _validationParameters, out _);
                return principal;
            }
            catch
            {
                throw new UnauthorizedAccessException("Invalid refresh token");
            }
        }
    }
}