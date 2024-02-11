using MediatR;

namespace FeedbackAnalysis.Application.FeedbackAnalyze.CreateFeedback
{
    public sealed class CreateFeedbackCommand : IRequest
    {
        public CreateFeedbackCommand(
            string feedback,
            string email)
        {
            Feedback = feedback;
            Email = email;
        }

        public string Feedback { get; }

        public string Email { get; }
    }
}
