using EScinece.Domain.Entities;
using EScinece.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EScinece.Infrastructure.Data;

public class EScienceDbContext : DbContext
{
    private readonly IConfiguration _configuration;
    public DbSet<User> User { get; init; }
    public DbSet<Account> Account { get; init; }
    public DbSet<Article> Article { get; init; }
    public DbSet<ArticleBranch> ArticleBranch { get; init; }
    public DbSet<ArticleBranchVersion> ArticleBranchVersion { get; init; }
    public DbSet<ArticleParticipant> ArticleParticipant { get; init; }
    
    public EScienceDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString(nameof(EScienceDbContext)));
        
        base.OnConfiguring(optionsBuilder);
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