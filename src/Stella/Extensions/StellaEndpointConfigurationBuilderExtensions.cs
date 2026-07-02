using System.Reflection;
using System.Runtime.CompilerServices;
using Stella.Handlers;

namespace Stella.Extensions;

public static class StellaEndpointConfigurationBuilderExtensions
{
    public static StellaEndpointConfigurationBuilder RegisterHandlersFromAssembly(this StellaEndpointConfigurationBuilder builder, Assembly assembly)
    {
        var handlerTypes = assembly.GetTypes()
            .Where(x => x is { IsClass: true, IsAbstract: false } && typeof(UpdateHandler).IsAssignableFrom(x));

        foreach (var handlerType in handlerTypes)
        {
            var handler = (IConfigurableHandler)RuntimeHelpers.GetUninitializedObject(handlerType);
            handler.Configure();

            builder.RegisterHandler(new BotEndpointDescriptor
            {
                HandlerType = handlerType,
                Priority = handler.Priority,
                ConditionDescriptors = handler.ConditionDescriptors ?? [],
                MiddlewareDescriptors = handler.MiddlewareDescriptors ?? [],
            });
        }

        return builder;
    }
}