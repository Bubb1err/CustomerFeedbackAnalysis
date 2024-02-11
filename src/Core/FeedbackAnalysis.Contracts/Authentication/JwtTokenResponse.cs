namespace FeedbackAnalysis.Contracts.Authentication
{
    public sealed class JwtTokenResponse
    {
        public string Token { get; set; }

        public string RefreshToken { get; set; }

        public DateTime ExpiresAt { get; set; }
    }
}
