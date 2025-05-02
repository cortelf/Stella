using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Stella.Extensions;

namespace Stella.Polling;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddStellaPolling(this IServiceCollection services,  Action<StellaPollingAppConfiguration>? cfgAction = null, Action<BotUpdateRouterConfigurationBuilder>? builderAction = null, params Assembly[] assemblies)
    {
        services.AddStellaPollingServices(cfgAction);
        services.AddStellaCore(builderAction, assemblies);

        return services;
    }
    
    public static IServiceCollection AddStellaPolling(this IServiceCollection services,  Action<StellaPollingAppConfiguration>? cfgAction = null, Action<BotUpdateRouterConfigurationBuilder>? builderAction = null)
    {
        services.AddStellaPollingServices(cfgAction);
        services.AddStellaCore(builderAction);

        return services;
    }

    private static IServiceCollection AddStellaPollingServices(this IServiceCollection services, Action<StellaPollingAppConfiguration>? cfgAction)
    {
        services.AddHostedService<StellaPollingHostedService>();
        var cfg = new StellaPollingAppConfiguration();
        cfgAction?.Invoke(cfg);

        services.AddSingleton(cfg);

        services.AddSingleton<IStellaPollingApp, StellaPollingApp>();

        return services;
    }
}