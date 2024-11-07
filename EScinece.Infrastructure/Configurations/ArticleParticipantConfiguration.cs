using EScinece.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EScinece.Infrastructure.Configurations;

internal class ArticleParticipantConfiguration : IEntityTypeConfiguration<ArticleParticipant>
{
    public void Configure(EntityTypeBuilder<ArticleParticipant> builder)
    {
        builder.HasKey(a => a.Id);
        
        builder
            .HasOne(a => a.Account)
            .WithMany(b => b.ArticleParticipants)
            .HasForeignKey(a => a.AccountId);

        builder
            .HasOne(a => a.Article)
            .WithOne(b => b.Creator)
            .HasForeignKey<ArticleParticipant>(a => a.ArticleId);
    }
}