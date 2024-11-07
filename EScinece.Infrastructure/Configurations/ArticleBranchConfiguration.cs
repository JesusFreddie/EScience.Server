using EScinece.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EScinece.Infrastructure.Configurations;

internal class ArticleBranchConfiguration : IEntityTypeConfiguration<ArticleBranch>
{
    public void Configure(EntityTypeBuilder<ArticleBranch> builder)
    {
        builder.HasKey(a => a.Id);
        
        builder
            .HasMany(a => a.ArticleBranchVersions)
            .WithOne(b => b.ArticleBranch)
            .HasForeignKey(b => b.ArticleBranchId);
        
        builder
            .HasOne(a => a.Article)
            .WithMany(b => b.ArticleBranches)
            .HasForeignKey(a => a.ArticleId);
        
        builder
            .HasOne(a => a.Creator)
            .WithMany(b => b.ArticleBranches)
            .HasForeignKey(a => a.CreatorId);
    }
}