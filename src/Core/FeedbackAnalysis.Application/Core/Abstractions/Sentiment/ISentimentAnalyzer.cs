namespace FeedbackAnalysis.Application.Core.Abstractions.Sentiment
{
    public interface ISentimentAnalyzer
    {
        (double positive, double negative, double neutral) ProcessAsync(string feedback);
    }
}
