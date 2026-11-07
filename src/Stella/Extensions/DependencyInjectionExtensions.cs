using Microsoft.Extensions.DependencyInjection;
using Stella.Routing;

namespace Stella.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddStellaCore(this IServiceCollection serviceCollection, Action<StellaEndpointConfigurationBuilder>? builderAction = null)
    {
        var builder = new StellaEndpointConfigurationBuilder();
        builderAction?.Invoke(builder);
        builder.ApplyConfiguration(serviceCollection);
        return serviceCollection
            .AddStellaCoreInterfaces();
    }
    
    private static IServiceCollection AddStellaCoreInterfaces(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IBotUpdateRouter, BotEndpointRouter>();

        return serviceCollection;
    }
}