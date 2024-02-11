using FeedbackAnalysis.Application.Core.Abstractions.Sentiment;
using FeedbackAnalysis.Domain.Abstractions.Repository.Base;
using FeedbackAnalysis.Domain.Entities;
using FeedbackAnalysis.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FeedbackAnalysis.Application.FeedbackAnalyze.CreateFeedback
{
    internal class CreateFeedbackCommandHandler : IRequestHandler<CreateFeedbackCommand>
    {
        private readonly ISentimentAnalyzer _sentimentAnalyzer;
        private readonly UserManager<User> _userManager;
        private readonly IRepository<Feedback> _feedbackRepository;

        public CreateFeedbackCommandHandler(
            ISentimentAnalyzer sentimentAnalyzer,
            UserManager<User> userManager,
            IRepository<Feedback> feedbackRepository)
        {
            _sentimentAnalyzer = sentimentAnalyzer;
            _userManager = userManager;
            _feedbackRepository = feedbackRepository;
        }
        public async Task Handle(CreateFeedbackCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Feedback))
            {
                throw new ExecutingException("Feedback is required", System.Net.HttpStatusCode.BadRequest);
            }

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                throw new ExecutingException("User was not found", System.Net.HttpStatusCode.Unauthorized);
            }

            var (positive, negative, neutral) = _sentimentAnalyzer.ProcessAsync(request.Feedback);

            var feedback = new Feedback
            {
                FeedbackMessage = request.Feedback,
                PositiveScore = positive,
                NegativeScore = negative,
                NeutralScore = neutral,
                Created = DateTime.UtcNow,
                FeedbackUserId = user.Id
            };

            await _feedbackRepository.AddAsync(feedback);
            await _feedbackRepository.SaveChangesAsync();
        }
    }
}
