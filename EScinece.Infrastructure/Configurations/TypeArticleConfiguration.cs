using EScinece.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EScinece.Infrastructure.Configurations;

public class TypeArticleConfiguration : IEntityTypeConfiguration<TypeArticle>
{
    public void Configure(EntityTypeBuilder<TypeArticle> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder
            .HasIndex(x => x.Name)
            .IsUnique();
    }
}