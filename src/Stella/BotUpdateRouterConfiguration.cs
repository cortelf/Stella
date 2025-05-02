namespace Stella;

public class BotUpdateRouterConfiguration: BotActionPointDescriptor
{
    public required IReadOnlyList<BotControllerDescriptor> ControllerDescriptors { get; init; }
}