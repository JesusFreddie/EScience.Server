using EScinece.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EScinece.Infrastructure.Configurations;

internal class UserConfiguration: IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(user => user.Id);

        builder
            .HasOne(user => user.Account)
            .WithOne(account => account.User);
    }
}