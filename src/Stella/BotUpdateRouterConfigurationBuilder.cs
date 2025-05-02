using Microsoft.Extensions.DependencyInjection;
using Stella.Conditions;
using Stella.Middlewares;

namespace Stella;

public class BotUpdateRouterConfigurationBuilder(IServiceCollection serviceCollection)
{
    private readonly List<ICondition> _conditions = [];
    private readonly List<ITypedCondition> _typedConditions = [];
    private readonly List<IMiddleware> _middlewares = [];
    private readonly List<ITypedMiddleware> _typedMiddlewares = [];
    
    public BotUpdateRouterConfigurationBuilder AddGlobalMiddleware(IMiddleware middleware)
    {
        _middlewares.Add(middleware);
        return this;
    }
    public BotUpdateRouterConfigurationBuilder AddGlobalTypedMiddleware(ITypedMiddleware middleware)
    {
        serviceCollection.AddScoped(middleware.Middleware);
        _typedMiddlewares.Add(middleware);
        return this;
    }
    
    public BotUpdateRouterConfigurationBuilder AddGlobalCondition(ICondition condition)
    {
        _conditions.Add(condition);
        return this;
    }
    public BotUpdateRouterConfigurationBuilder AddGlobalTypedCondition(ITypedCondition condition)
    {
        serviceCollection.AddScoped(condition.Condition);
        _typedConditions.Add(condition);
        return this;
    }

    public BotUpdateRouterConfiguration Build()
    {
        var actionPointLoader = new BotActionPointDescriptorLoader();
        var handlerFinder = new BotControllerHandlerFinder();
        var descriptorLoader = new BotControllerDescriptorLoader(serviceCollection, actionPointLoader, handlerFinder);

        return new BotUpdateRouterConfiguration()
        {
            Conditions = _conditions,
            Middlewares = _middlewares,
            TypedMiddlewares = _typedMiddlewares,
            TypedConditions = _typedConditions,
            ControllerDescriptors = descriptorLoader.Load()
        };
    }
}