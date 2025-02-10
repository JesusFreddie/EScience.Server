using EScience.Application.Configuration;
using EScience.Application.Extensions;
using EScience.Application.Handlers;
using EScinece.Domain.Abstraction.Helpers;
using EScinece.Infrastructure.Data;
using EScinece.Infrastructure.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.Extensions.Options;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
services.AddApiAuthentication(services.BuildServiceProvider().GetRequiredService<IOptions<JwtOptions>>());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IDbConnectionFactory, EScienceDbContext>(provider => 
    new EScienceDbContext(
        provider.GetRequiredService<ILogger<EScienceDbContext>>(),
        configuration.GetConnectionString(nameof(EScienceDbContext))!));
builder.Services.AddStackExchangeRedisCache(options => options.Configuration = "localhost:6380");
Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.File("logs/escience.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Services.AddScoped<IAuthorizationHandler, ArticleAuthorizationHandler>();

builder.Services.AddRepositories();
builder.Services.AddServices();

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