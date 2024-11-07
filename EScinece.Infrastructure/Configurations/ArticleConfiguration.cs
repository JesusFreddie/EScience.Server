using EScinece.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EScinece.Infrastructure.Configurations;

internal class ArticleConfiguration: IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .HasOne(article => article.Creator)
            .WithOne(articleParticipant => articleParticipant.Article)
            .HasForeignKey<Article>(article => article.CreatorId);

        builder
            .HasMany(acticle => acticle.ArticleBranches)
            .WithOne(branches => branches.Article)
            .HasForeignKey(branches => branches.ArticleId);
    }
}