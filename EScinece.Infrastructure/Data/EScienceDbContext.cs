using EScinece.Domain.Entities;
using EScinece.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace EScinece.Infrastructure.Data;

public class EScienceDbContext : DbContext
{
    public DbSet<User> User { get; set; }
    public DbSet<Account> Account { get; set; }
    public DbSet<Article> Article { get; set; }
    public DbSet<ArticleBranch> ArticleBranch { get; set; }
    public DbSet<ArticleBranchVersion> ArticleBranchVersion { get; set; }
    public DbSet<ArticleParticipant> ArticleParticipant { get; set; }
    
    public EScienceDbContext(DbContextOptions<EScienceDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AccountConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new ArticleConfiguration());
        modelBuilder.ApplyConfiguration(new ArticleBranchConfiguration());
        modelBuilder.ApplyConfiguration(new ArticleBranchVersionConfiguration());
        modelBuilder.ApplyConfiguration(new ArticleParticipantConfiguration());
        
        base.OnModelCreating(modelBuilder);
    }
}