using FeedbackAnalysis.Application.Core.Abstractions.Authentication;
using FeedbackAnalysis.Application.Core.Abstractions.Sentiment;
using FeedbackAnalysis.Infrastructure.Authentication;
using FeedbackAnalysis.Infrastructure.Sentiment;
using Microsoft.Extensions.DependencyInjection;

namespace FeedbackAnalysis.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IJwtProvider, JwtProvider>();
            services.AddScoped<ISentimentAnalyzer, SentimentAnalyzer>();

            return services;
        }
    }
}
