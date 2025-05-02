using System.Reflection;

namespace Stella.Interfaces;

public interface IBotControllerHandlerFinder
{
    IReadOnlyList<MethodInfo> Find(Type type);
}