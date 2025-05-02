using Microsoft.Extensions.DependencyInjection;
using Stella.Extensions;
using Stella.Interfaces;
using Telegram.Bot.Types;

namespace Stella;

public class BotUpdateRouter(BotUpdateRouterConfiguration configuration, IBotUpdateProcessor botUpdateProcessor, IServiceProvider serviceProvider): IBotUpdateRouter
{
    public async Task RouteAsync(Update update, CancellationToken cancellationToken = default)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var globalConditions = configuration.GetConditions(scope.ServiceProvider);
        
        if (!globalConditions.All(x => x.Check(update)))
            return;

        foreach (var controllerDescriptor in configuration.ControllerDescriptors)
        {
            var controllerConditions = controllerDescriptor.GetConditions(scope.ServiceProvider);
            
            if (!controllerConditions.All(x => x.Check(update)))
                continue;

            foreach (var handlerDescriptor in controllerDescriptor.HandlerDescriptors)
            {
                var handlerConditions = handlerDescriptor.GetConditions(scope.ServiceProvider);
                if (!handlerConditions.All(x => x.Check(update)))
                    continue;
                
                var globalMiddlewares = configuration.GetMiddlewares(scope.ServiceProvider);
                var controllerMiddlewares = controllerDescriptor.GetMiddlewares(scope.ServiceProvider);
                var handlerMiddlewares = handlerDescriptor.GetMiddlewares(scope.ServiceProvider);

                var middlewares = globalMiddlewares.Concat(controllerMiddlewares).Concat(handlerMiddlewares);

                var controllerInstance = scope.ServiceProvider.GetRequiredService(controllerDescriptor.Type);
                
                await botUpdateProcessor.ProcessAsync(update, handlerDescriptor.Method, controllerInstance, middlewares.ToList(),
                    cancellationToken);
                
                return;
            }
        }
        
    }
}