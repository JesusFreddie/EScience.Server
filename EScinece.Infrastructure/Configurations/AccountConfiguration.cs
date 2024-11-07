using EScinece.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EScinece.Infrastructure.Configurations;

internal class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .HasOne(account => account.User)
            .WithOne(user => user.Account);

        builder
            .HasMany(account => account.ArticleParticipants)
            .WithOne(articleParticipant => articleParticipant.Account)
            .HasForeignKey(articleParticipant => articleParticipant.AccountId);
    }
}