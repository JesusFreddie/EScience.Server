using EScinece.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EScinece.Infrastructure.Configurations;

public class ArticleBranchVersionConfiguration: IEntityTypeConfiguration<ArticleBranchVersion>
{
    public void Configure(EntityTypeBuilder<ArticleBranchVersion> builder)
    {
        builder.HasKey(a => a.Id);
        
        builder
            .HasOne(a => a.Creator)
            .WithMany(b => b.ArticleBranchVersions)
            .HasForeignKey(a => a.CreatorId);
        
        builder
            .HasOne(a => a.ArticleBranch)
            .WithMany(b => b.ArticleBranchVersions)
            .HasForeignKey(a => a.ArticleBranchId);
    }
}