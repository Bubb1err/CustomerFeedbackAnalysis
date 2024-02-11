using FeedbackAnalysis.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FeedbackAnalysis.Persistance.Configurations
{
    internal sealed class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasMany<Feedback>()
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
