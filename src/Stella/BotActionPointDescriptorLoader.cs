using System.Reflection;
using Stella.Conditions;
using Stella.Interfaces;
using Stella.Middlewares;

namespace Stella;

public class BotActionPointDescriptorLoader: IBotActionPointDescriptorLoader
{
    public BotActionPointDescriptor Load(MemberInfo memberInfo)
    {
        var conditions = memberInfo
            .GetCustomAttributes(typeof(ConditionAttribute<>), inherit: true)
            .Select(x => x as ICondition).Where(x => x is not null)
            .Cast<ICondition>();
        var typedConditions = memberInfo
            .GetCustomAttributes(typeof(ConditionAttribute<>), inherit: true)
            .Where(x => x as ICondition is null)
            .Cast<ITypedCondition>();
        
        var middlewares = memberInfo
            .GetCustomAttributes(typeof(MiddlewareAttribute<>), inherit: true)
            .Select(x => x as IMiddleware).Where(x => x is not null)
            .Cast<IMiddleware>();
        var typedMiddlewares = memberInfo
            .GetCustomAttributes(typeof(MiddlewareAttribute<>), inherit: true)
            .Where(x => x as IMiddleware is null)
            .Cast<ITypedMiddleware>();

        return new BotActionPointDescriptor
        {
            Conditions = conditions.ToList(),
            TypedConditions = typedConditions.ToList(),
            Middlewares = middlewares.ToList(),
            TypedMiddlewares = typedMiddlewares.ToList()
        };
    }
}