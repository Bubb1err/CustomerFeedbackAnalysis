using Azure;
using Azure.AI.TextAnalytics;
using FeedbackAnalysis.Application.Core.Abstractions.Sentiment;
using FeedbackAnalysis.Domain.Exceptions;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace FeedbackAnalysis.Infrastructure.Sentiment
{
    public sealed class SentimentAnalyzer : ISentimentAnalyzer
    {
        private readonly IConfiguration _configuration;

        public SentimentAnalyzer(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Analyzes user feedback text to determine sentiment and returns metrics indicating the positivity, negativity, and neutrality of the feedback.
        /// </summary>
        /// <param name="feedback">The feedback text to be analyzed.</param>
        /// <returns>
        /// A tuple containing three double values representing the level of positivity, negativity, and neutrality of the feedback.
        /// The values range from 0 to 1, where 0 indicates low sentiment and 1 indicates high sentiment.
        /// </returns>
        /// <exception cref="ExecutingException">Thrown when an error occurs during the feedback analysis process.</exception>
        public (double positive, double negative, double neutral) ProcessAsync(string feedback)
        {
            string analyticsServiceKey = _configuration["AnalysisKey"];
            string analyticsServiceEndpoint = _configuration["AnalysisEndpoint"];

            AzureKeyCredential credentials = new AzureKeyCredential(analyticsServiceKey);
            Uri endpoint = new Uri(analyticsServiceEndpoint);

            var client = new TextAnalyticsClient(endpoint, credentials);

            var documents = new List<string>
            {
                feedback
            };

            AnalyzeSentimentResultCollection reviews = client.AnalyzeSentimentBatch(documents, options: new AnalyzeSentimentOptions()
            {
                IncludeOpinionMining = true
            });

            var review = reviews.FirstOrDefault();

            if (review is null)
            {
                throw new ExecutingException("Error occured while processing feedback", HttpStatusCode.BadRequest);
            }

            return (review.DocumentSentiment.ConfidenceScores.Positive,
                review.DocumentSentiment.ConfidenceScores.Negative,
                review.DocumentSentiment.ConfidenceScores.Neutral);
        }
    }
}
