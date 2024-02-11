using FeedbackAnalysis.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FeedbackAnalysis.Persistance.Configurations
{
    internal sealed class RefreshTokenEntityTypeConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(token => token.Id);

            builder.Property(token => token.Token).IsRequired();

            builder.Property(token => token.JwtId).IsRequired();

            builder.Property(token => token.IsRevoked).IsRequired();

            builder.Property(token => token.DateAdded).IsRequired();

            builder.Property(token => token.DateExpired).IsRequired();

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(token => token.UserId);
        }
    }
}
