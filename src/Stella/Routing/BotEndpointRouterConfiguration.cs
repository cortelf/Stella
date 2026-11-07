using Stella.Handlers;

namespace Stella.Routing;

internal class BotEndpointRouterConfiguration
{
    public required IList<BotEndpointDescriptor> HandlerDescriptors { get; init; }
    public required IList<MiddlewareDescriptorContainer> GlobalMiddlewareDescriptors { get; init; }
}