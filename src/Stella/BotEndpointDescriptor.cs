using Stella.Handlers;

namespace Stella;

public class BotEndpointDescriptor
{
    public required Type HandlerType { get; init; }
    public required int Priority { get; init; }
    public required IList<ConditionDescriptorContainer> ConditionDescriptors { get; init; }
    public required IList<MiddlewareDescriptorContainer> MiddlewareDescriptors { get; init; }
}