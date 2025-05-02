using System.Reflection;
using Stella.Conditions;
using Stella.Middlewares;

namespace Stella;

public class BotActionPointDescriptor
{
    public required IReadOnlyList<ICondition> Conditions { get; init; }
    public required IReadOnlyList<ITypedCondition> TypedConditions { get; init; }
    public required IReadOnlyList<IMiddleware> Middlewares { get; init; }
    public required IReadOnlyList<ITypedMiddleware> TypedMiddlewares { get; init; }
}

public class BotControllerHandlerDescriptor : BotActionPointDescriptor
{
    public required MethodInfo Method { get; init; }
}

public class BotControllerDescriptor: BotActionPointDescriptor
{
    public required Type Type { get; init; }
    
    public required IReadOnlyList<BotControllerHandlerDescriptor> HandlerDescriptors { get; init; }
}