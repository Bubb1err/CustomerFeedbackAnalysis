using AutoMapper;
using FeedbackAnalysis.Domain.Entities;

namespace FeedbackAnalysis.Contracts.FeedbackModels.FeedbackMappings
{
    public class FeedbackMapping : Profile
    {
        public FeedbackMapping()
        {
            CreateMap<Feedback, DisplayFeedback>();
        }
    }
}
