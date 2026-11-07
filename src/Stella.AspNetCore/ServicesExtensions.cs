using Stella.Extensions;

namespace Stella.AspNetCore;

public static class ServicesExtensions
{
    public static IServiceCollection AddStella(this IServiceCollection services, Action<StellaEndpointConfigurationBuilder>? builderAction = null)
    {
        services.AddStellaCore(builderAction);

        return services;
    }
}