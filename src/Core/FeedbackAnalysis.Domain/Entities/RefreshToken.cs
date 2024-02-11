namespace FeedbackAnalysis.Domain.Entities
{
    public sealed class RefreshToken
    {
        public Guid Id { get; set; }

        public string Token { get; set; }

        public string JwtId { get; set; }

        public bool IsRevoked { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime DateExpired { get; set; }

        public string UserId { get; set; }
    }
}
