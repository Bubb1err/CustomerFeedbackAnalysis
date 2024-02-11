using FeedbackAnalysis.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FeedbackAnalysis.Persistance.Configurations
{
    internal sealed class FeedbackEntityTypeConfiguration : IEntityTypeConfiguration<Feedback>
    {
        public void Configure(EntityTypeBuilder<Feedback> builder)
        {
            builder.HasKey(feedback => feedback.Id);

            builder.Property(feedback => feedback.FeedbackMessage).IsRequired();

            builder.Property(feedback => feedback.NegativeScore).IsRequired();

            builder.Property(feedback => feedback.PositiveScore).IsRequired();

            builder.Property(feedback => feedback.NeutralScore).IsRequired();

            builder.Property(feedback => feedback.Created).IsRequired();

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(feedback => feedback.FeedbackUserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
