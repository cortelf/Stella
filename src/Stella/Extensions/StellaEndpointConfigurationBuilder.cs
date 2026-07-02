using Microsoft.Extensions.DependencyInjection;
using Stella.Handlers;
using Stella.Middlewares;
using Stella.Routing;

namespace Stella.Extensions;

public class StellaEndpointConfigurationBuilder
{
    private readonly IList<BotEndpointDescriptor> _handlerDescriptors = [];
    private readonly IList<MiddlewareDescriptorContainer> _globalMiddlewareContainers = [];

    public StellaEndpointConfigurationBuilder RegisterHandler(BotEndpointDescriptor descriptor)
    {
        _handlerDescriptors.Add(descriptor);
        return this;
    }
    
    public StellaEndpointConfigurationBuilder RegisterGlobalMiddleware(MiddlewareDescriptorContainer descriptor)
    {
        _globalMiddlewareContainers.Add(descriptor);
        return this;
    }
    
    public StellaEndpointConfigurationBuilder RegisterGlobalMiddleware<TMiddleware>(object? context)
        where TMiddleware: IAsyncMiddleware
    {
        _globalMiddlewareContainers.Add(new MiddlewareDescriptorContainer(typeof(TMiddleware), context));
        return this;
    }
    
    public StellaEndpointConfigurationBuilder RegisterGlobalMiddleware<TMiddleware, TContext>(TContext context)
        where TMiddleware: AsyncMiddleware<TContext>
    {
        _globalMiddlewareContainers.Add(new MiddlewareDescriptorContainer(typeof(TMiddleware), context));
        return this;
    }
    
    public StellaEndpointConfigurationBuilder RegisterGlobalMiddleware<TMiddleware>()
        where TMiddleware: AsyncMiddleware
    {
        _globalMiddlewareContainers.Add(new MiddlewareDescriptorContainer(typeof(TMiddleware), null));
        return this;
    }

    public void ApplyConfiguration(IServiceCollection services)
    {
        var handlerTypes = _handlerDescriptors.Select(x => x.HandlerType).Distinct().ToList();
        foreach (var handlerType in handlerTypes) 
            services.AddScoped(handlerType);
        var conditionTypes = _handlerDescriptors.SelectMany(x => x.ConditionDescriptors)
            .Select(x => x.ConditionType).Distinct().ToList();
        foreach (var conditionType in conditionTypes)
            services.AddScoped(conditionType);
        var handlerMiddlewareTypes = _handlerDescriptors.SelectMany(x => x.MiddlewareDescriptors)
            .Select(x => x.MiddlewareType).Distinct().ToList();
        foreach (var middlewareType in handlerMiddlewareTypes)
            services.AddScoped(middlewareType);
        var globalMiddlewareTypes = _globalMiddlewareContainers.Select(x =>x.MiddlewareType)
            .Distinct().ToList();
        foreach (var middlewareType in globalMiddlewareTypes)
            services.AddScoped(middlewareType);

        var configuration = new BotEndpointRouterConfiguration
        {
            GlobalMiddlewareDescriptors = _globalMiddlewareContainers,
            HandlerDescriptors = _handlerDescriptors
        };
        
        services.AddSingleton(configuration);
    }
}