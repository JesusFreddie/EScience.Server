using EScinece.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using EScience.Application.Handlers;
namespace EScience.Application.Configuration;

public static class PolicyConfiguration
{
    public static IServiceCollection AddPolicies(this IServiceCollection service)
    {
        service.AddScoped<IAuthorizationHandler, ArticlePolicyHandler>();
        return service;
    }
}