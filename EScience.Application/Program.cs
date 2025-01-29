using EScience.Application.Extensions;
using EScinece.Domain.Abstraction;
using EScinece.Domain.Abstraction.Helpers;
using EScinece.Domain.Abstraction.Repositories;
using EScinece.Domain.Abstraction.Services;
using EScinece.Domain.Entities;
using EScinece.Infrastructure.Data;
using EScinece.Infrastructure.Helpers;
using EScinece.Infrastructure.Repositories;
using EScinece.Infrastructure.Services;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
services.AddApiAuthentication(services.BuildServiceProvider().GetRequiredService<IOptions<JwtOptions>>());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddDbContext<EScienceDbContext>(options => options.UseNpgsql(configuration.GetConnectionString(nameof(EScienceDbContext))));
builder.Services.AddStackExchangeRedisCache(options => options.Configuration = "localhost:6380");

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<IArticleParticipantRepository, ArticleParticipantRepository>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IArticleParticipantService, ArticleParticipantService>();
builder.Services.AddScoped<IArticleService, ArticleService>();

builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCookiePolicy(new CookiePolicyOptions()
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always
});

app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
        );

app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.Run();