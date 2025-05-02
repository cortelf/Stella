using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Stella.Conditions;
using Stella.Interfaces;
using Stella.Middlewares;

namespace Stella.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddStellaCore(this IServiceCollection serviceCollection, Action<BotUpdateRouterConfigurationBuilder>? builderAction = null, params Assembly[] assemblies)
    {
        return serviceCollection
            .AddStellaCoreHandlers(assemblies)
            .AddStellaCoreMiddlewares(assemblies)
            .AddStellaCoreConditions(assemblies)
            .AddStellaCoreDescriptors(builderAction)
            .AddStellaCoreInterfaces();
    }
    
    public static IServiceCollection AddStellaCore(this IServiceCollection serviceCollection, Action<BotUpdateRouterConfigurationBuilder>? builderAction = null)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        return serviceCollection.AddStellaCore(builderAction, assemblies);
    }
    
    private static IServiceCollection AddStellaCoreDescriptors(this IServiceCollection serviceCollection, Action<BotUpdateRouterConfigurationBuilder>? builderAction)
    {
        var builder = new BotUpdateRouterConfigurationBuilder(serviceCollection);
        builderAction?.Invoke(builder);
        
        serviceCollection.AddSingleton(builder.Build());

        return serviceCollection;
    }
    
    private static IServiceCollection AddStellaCoreInterfaces(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IBotUpdateRouter, BotUpdateRouter>();
        serviceCollection.AddSingleton<IBotUpdateProcessor, BotUpdateProcessor>();

        return serviceCollection;
    }

    private static IServiceCollection AddStellaCoreHandlers(this IServiceCollection serviceCollection, params Assembly[] assemblies)
    {
        var types = assemblies
            .SelectMany(x => x.GetTypes())
            .Where(x => x.GetCustomAttribute<BotControllerAttribute>(inherit: true) is not null);

        foreach (var typeToAdd in types)
        {
            serviceCollection.AddScoped(typeToAdd);
        }

        return serviceCollection;
    }

    private static IServiceCollection AddStellaCoreMiddlewares(this IServiceCollection serviceCollection, params Assembly[] assemblies)
    {
        var types = assemblies
            .SelectMany(x => x.GetTypes())
            .SelectMany(x => x.GetMethods().Cast<MemberInfo>().Concat([x]))
            .SelectMany(x => x.GetCustomAttributes(typeof(MiddlewareAttribute<>), inherit: false))
            .Where(x => x as IMiddleware is null)
            .Select(x => (ITypedMiddleware)x)
            .ToHashSet();

        foreach (var typeToAdd in types)
        {
            serviceCollection.AddScoped(typeToAdd.Middleware);
        }

        return serviceCollection;
    }

    private static IServiceCollection AddStellaCoreConditions(this IServiceCollection serviceCollection, params Assembly[] assemblies)
    {
        var types = assemblies
            .SelectMany(x => x.GetTypes())
            .SelectMany(x => x.GetMethods().Cast<MemberInfo>().Concat([x]))
            .SelectMany(x => x.GetCustomAttributes(typeof(ConditionAttribute<>), inherit: false))
            .Where(x => x as ICondition is null)
            .Select(x => (ITypedCondition)x)
            .ToHashSet();

        foreach (var typeToAdd in types)
        {
            serviceCollection.AddScoped(typeToAdd.Condition);
        }

        return serviceCollection;
    }
}