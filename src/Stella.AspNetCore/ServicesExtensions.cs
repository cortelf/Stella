using System.Reflection;
using Stella.Extensions;

namespace Stella.AspNetCore;

public static class ServicesExtensions
{
    public static IServiceCollection AddStella(this IServiceCollection services, Action<BotUpdateRouterConfigurationBuilder>? builderAction = null, params Assembly[] assemblies)
    {
        services.AddStellaCore(builderAction, assemblies);

        return services;
    }
    
    public static IServiceCollection AddStella(this IServiceCollection services, Action<BotUpdateRouterConfigurationBuilder>? builderAction = null)
    {
        services.AddStellaCore(builderAction);

        return services;
    }
}