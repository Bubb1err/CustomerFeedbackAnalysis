namespace FeedbackAnalysis.Web.Models.Authentication
{
    public class TokenResponseViewModel
    {
        public string Token { get; set; }

        public string RefreshToken { get; set; }

        public DateTime ExpiresAt { get; set; }
    }
}
