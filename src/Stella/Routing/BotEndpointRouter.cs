using Microsoft.Extensions.DependencyInjection;
using Stella.Conditions;
using Stella.Handlers;
using Stella.Middlewares;
using Telegram.Bot.Types;

namespace Stella.Routing;

internal class BotEndpointRouter(IServiceProvider serviceProvider, BotEndpointRouterConfiguration configuration): IBotUpdateRouter
{
    public async Task RouteAsync(Update update, CancellationToken cancellationToken = default)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        IUpdateHandler? handler = null;
        BotEndpointDescriptor? endpointDescriptor = null;

        foreach (var handlerDescriptor in configuration.HandlerDescriptors.OrderBy(x => x.Priority))
        {
            var conditionContainers = handlerDescriptor.ConditionDescriptors.Select(x =>
                new ConditionContainer((IAsyncCondition)serviceProvider.GetRequiredService(x.ConditionType), x.Context));
            var handlerCandidate = (IUpdateHandler)serviceProvider.GetRequiredService(handlerDescriptor.HandlerType);
            handlerCandidate.Conditions = conditionContainers.OrderBy(x => x.Condition.Priority).ToList();
            if (await handlerCandidate.CanHandleAsync(update, cancellationToken))
            {
                handler = handlerCandidate;
                endpointDescriptor = handlerDescriptor;
                break;
            }
        }
        
        if (handler == null)
            return;
        
        var middlewareContainers = endpointDescriptor!.MiddlewareDescriptors
            .Union(configuration.GlobalMiddlewareDescriptors).Select(x =>
            new MiddlewareContainer((IAsyncMiddleware)serviceProvider.GetRequiredService(x.MiddlewareType), x.Context));
        handler.Middlewares = middlewareContainers.OrderBy(x => x.Middleware.Priority).ToList();
        await handler.HandleAsync(update, cancellationToken);
    }
}