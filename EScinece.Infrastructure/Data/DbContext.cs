using EScinece.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EScinece.Infrastructure.Data;

public class EScienceContext: DbContext
{
    public DbSet<User> User { get; set; }
    public DbSet<Account> Account { get; set; }
    
    public EScienceContext(DbContextOptions options) : base(options)
    {
    }
}