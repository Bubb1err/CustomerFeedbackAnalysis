using FeedbackAnalysis.Contracts.Authentication;
using MediatR;

namespace FeedbackAnalysis.Application.Authentication.RefreshTokenHandle
{
    public sealed class RefreshTokenCommand : IRequest<JwtTokenResponse>
    {
        public RefreshTokenCommand(string refreshToken)
        {
            RefreshToken = refreshToken;
        }

        public string RefreshToken { get; }
    }
}
