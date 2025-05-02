namespace Stella.Interfaces;

public interface IBotControllerDescriptorLoader
{
    IReadOnlyList<BotControllerDescriptor> Load();
}