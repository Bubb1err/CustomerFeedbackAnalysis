using FeedbackAnalysis.Contracts.FeedbackModels;
using MediatR;

namespace FeedbackAnalysis.Application.FeedbackAnalyze.GetFeedback
{
    public sealed class GetFeedbackCommand : IRequest<IEnumerable<DisplayFeedback>>
    {
    }
}
