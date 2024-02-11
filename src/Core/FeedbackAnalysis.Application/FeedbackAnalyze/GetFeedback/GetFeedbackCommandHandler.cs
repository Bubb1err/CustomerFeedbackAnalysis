using AutoMapper;
using FeedbackAnalysis.Contracts.FeedbackModels;
using FeedbackAnalysis.Domain.Abstractions.Repository.Base;
using FeedbackAnalysis.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FeedbackAnalysis.Application.FeedbackAnalyze.GetFeedback
{
    internal sealed class GetFeedbackCommandHandler : IRequestHandler<GetFeedbackCommand, IEnumerable<DisplayFeedback>>
    {
        private readonly IRepository<Feedback> _feedbackRepository;
        private readonly IMapper _mapper;
        private const int _feedbackGetAmount = 6;

        public GetFeedbackCommandHandler(
            IRepository<Feedback> feedbackRepository,
            IMapper mapper)
        {
            _feedbackRepository = feedbackRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DisplayFeedback>> Handle(GetFeedbackCommand request, CancellationToken cancellationToken)
        {
            var feedbacks = await _feedbackRepository.GetAll().OrderBy(feedback => feedback.Created).Take(_feedbackGetAmount).ToListAsync();
            return _mapper.Map<IEnumerable<DisplayFeedback>>(feedbacks);
        }
    }
}
