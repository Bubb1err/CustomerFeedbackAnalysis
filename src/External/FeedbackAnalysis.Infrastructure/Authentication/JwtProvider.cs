using FeedbackAnalysis.Application.Core.Abstractions.Authentication;
using FeedbackAnalysis.Contracts.Authentication;
using FeedbackAnalysis.Domain.Abstractions.Repository.Base;
using FeedbackAnalysis.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FeedbackAnalysis.Infrastructure.Authentication
{
    internal class JwtProvider : IJwtProvider
    {
        private readonly IConfiguration _configuration;
        private readonly IRepository<RefreshToken> _refreshTokenRepository;

        public JwtProvider(
            IConfiguration configuration,
            IRepository<RefreshToken> refreshTokenRepository)
        {
            _configuration = configuration;
            _refreshTokenRepository = refreshTokenRepository;
        }
        public async Task<JwtTokenResponse> GenerateJWTTokenAsync(User user, RefreshToken refreshToken)
        {
            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Name, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var token = new JwtSecurityToken(issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.UtcNow.AddMinutes(30),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));


            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            if (refreshToken != null)
            {
                var rTokenResponse = new JwtTokenResponse()
                {
                    Token = jwtToken,
                    RefreshToken = refreshToken.Token,
                    ExpiresAt = token.ValidTo
                };
                return rTokenResponse;
            }

            refreshToken = new RefreshToken()
            {
                JwtId = token.Id,
                IsRevoked = false,
                UserId = user.Id,
                DateAdded = DateTime.UtcNow,
                DateExpired = DateTime.UtcNow.AddMonths(6),
                Token = Guid.NewGuid().ToString() + "-" + Guid.NewGuid().ToString(),
            };
            await _refreshTokenRepository.AddAsync(refreshToken);
            await _refreshTokenRepository.SaveChangesAsync();

            var result = new JwtTokenResponse()
            {
                Token = jwtToken,
                RefreshToken = refreshToken.Token,
                ExpiresAt = token.ValidTo
            };
            return result;
        }
    }
}
