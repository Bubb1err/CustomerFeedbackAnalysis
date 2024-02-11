﻿namespace FeedbackAnalysis.Contracts.FeedbackModels
{
    public sealed class DisplayFeedback
    {
        public Guid Id { get; set; }

        public string FeedbackMessage { get; set; }

        public double PositiveScore { get; set; }

        public double NegativeScore { get; set; }

        public double NeutralScore { get; set; }

        public DateTime Created { get; set; }
    }
}
