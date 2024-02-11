using FeedbackAnalysis.Application.Core.Abstractions.Authentication;
using FeedbackAnalysis.Contracts.Authentication;
using FeedbackAnalysis.Domain.Abstractions.Repository.Base;
using FeedbackAnalysis.Domain.Entities;
using FeedbackAnalysis.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace FeedbackAnalysis.Application.Authentication.RefreshTokenHandle
{
    internal sealed class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, JwtTokenResponse>
    {
        private readonly IRepository<RefreshToken> _refreshTokenRepository;
        private readonly UserManager<User> _userManager;
        private readonly IJwtProvider _jwtProvider;

        public RefreshTokenCommandHandler(
            IRepository<RefreshToken> refreshTokenRepository,
            UserManager<User> userManager,
            IJwtProvider jwtProvider)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _userManager = userManager;
            _jwtProvider = jwtProvider;
        }

        public async Task<JwtTokenResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.RefreshToken))
            {
                throw new ExecutingException("Refresh token is required", System.Net.HttpStatusCode.BadRequest);
            }

            var storedToken = await _refreshTokenRepository.GetFirstOrDefaultAsync(x => x.Token == request.RefreshToken);
            if (storedToken == null)
            {
                throw new ExecutingException("Token was not found", System.Net.HttpStatusCode.Unauthorized);
            }

            var dbUser = await _userManager.FindByIdAsync(storedToken.UserId);

            try
            {
                return await _jwtProvider.GenerateJWTTokenAsync(dbUser, storedToken);
            }
            catch (SecurityTokenExpiredException)
            {
                if (storedToken.DateExpired >= DateTime.UtcNow)
                {
                    await _jwtProvider.GenerateJWTTokenAsync(dbUser, storedToken);
                }
                else
                {
                    await _jwtProvider.GenerateJWTTokenAsync(dbUser, null);
                }
            }

            throw new ExecutingException("Unexpected error occured.", System.Net.HttpStatusCode.BadRequest);
        }
    }
}
