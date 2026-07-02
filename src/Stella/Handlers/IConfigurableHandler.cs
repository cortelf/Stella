using Stella.Conditions;
using Stella.Middlewares;

namespace Stella.Handlers;

public class ConditionDescriptorContainer(Type conditionType, object? context)
{
    public Type ConditionType { get; }  = conditionType;
    public object? Context { get; }  = context;
}

public class MiddlewareDescriptorContainer(Type middlewareType, object? context)
{
    public Type MiddlewareType { get; } = middlewareType;
    public object? Context { get; }  = context;
}

internal interface IConfigurableHandler
{
    internal int Priority { get; }
    internal IList<ConditionDescriptorContainer>? ConditionDescriptors { get; }
    
    internal IList<MiddlewareDescriptorContainer>? MiddlewareDescriptors { get; }

    void Configure();
}