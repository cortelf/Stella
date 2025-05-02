using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Stella.Interfaces;

namespace Stella;

public class BotControllerDescriptorLoader(IServiceCollection serviceCollection, IBotActionPointDescriptorLoader actionPointDescriptorLoader, IBotControllerHandlerFinder methodFinder): IBotControllerDescriptorLoader
{
    public IReadOnlyList<BotControllerDescriptor> Load()
    {
        var controllers = serviceCollection
            .Where(x => x.ServiceType.GetCustomAttribute<BotControllerAttribute>() != null)
            .Select(x => x.ServiceType)
            .ToList();

        var result = new List<BotControllerDescriptor>(controllers.Count);

        foreach (var controllerType in controllers)
        {
            var controllerActionPointDescriptor = actionPointDescriptorLoader.Load(controllerType);
            var methods = methodFinder.Find(controllerType);
            if (methods.Count == 0)
                continue;
            result.Add(new BotControllerDescriptor
            {
                Type = controllerType,
                Middlewares = controllerActionPointDescriptor.Middlewares,
                TypedMiddlewares = controllerActionPointDescriptor.TypedMiddlewares,
                Conditions = controllerActionPointDescriptor.Conditions,
                TypedConditions = controllerActionPointDescriptor.TypedConditions,
                HandlerDescriptors = methods.Select(x =>
                {
                    var handlerActionPointDescriptor = actionPointDescriptorLoader.Load(x);
                    return new BotControllerHandlerDescriptor
                    {
                        Method = x,
                        Middlewares = handlerActionPointDescriptor.Middlewares,
                        TypedMiddlewares = handlerActionPointDescriptor.TypedMiddlewares,
                        Conditions = handlerActionPointDescriptor.Conditions,
                        TypedConditions = handlerActionPointDescriptor.TypedConditions,
                    };
                }).ToList()
            });
        }

        return result;
    }
}