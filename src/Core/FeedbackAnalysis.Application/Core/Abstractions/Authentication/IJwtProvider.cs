using FeedbackAnalysis.Contracts.Authentication;
using FeedbackAnalysis.Domain.Entities;

namespace FeedbackAnalysis.Application.Core.Abstractions.Authentication
{
    public interface IJwtProvider
    {
        Task<JwtTokenResponse> GenerateJWTTokenAsync(User user, RefreshToken refreshToken);
    }
}
