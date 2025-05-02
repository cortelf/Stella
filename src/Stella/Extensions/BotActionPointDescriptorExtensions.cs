using Microsoft.Extensions.DependencyInjection;
using Stella.Conditions;
using Stella.Middlewares;

namespace Stella.Extensions;

public static class BotActionPointDescriptorExtensions
{
    public static List<ICondition> GetConditions(this BotActionPointDescriptor actionPointDescriptor, IServiceProvider serviceProvider)
    {
        var typedInstances =
            actionPointDescriptor.TypedConditions.Select(x =>
                serviceProvider.GetRequiredService(x.Condition)).Cast<ICondition>();
        return actionPointDescriptor.Conditions.Concat(typedInstances).ToList();
    }
    
    public static List<IMiddleware> GetMiddlewares(this BotActionPointDescriptor actionPointDescriptor, IServiceProvider serviceProvider)
    {
        var typedInstances =
            actionPointDescriptor.TypedMiddlewares.Select(x =>
                serviceProvider.GetRequiredService(x.Middleware)).Cast<IMiddleware>();
        return actionPointDescriptor.Middlewares.Concat(typedInstances).ToList();
    }
}