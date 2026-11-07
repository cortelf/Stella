using Stella.Conditions;
using Stella.Middlewares;
using Telegram.Bot.Types;

namespace Stella.Handlers;

public class ConditionContainer(IAsyncCondition condition, object? context)
{
    public IAsyncCondition Condition { get; } = condition;
    public object? Context { get; } = context;
}

public class MiddlewareContainer(IAsyncMiddleware middleware, object? context)
{
    public IAsyncMiddleware Middleware { get; } = middleware;
    public object? Context { get; } = context;
}

public interface IUpdateHandler
{
    public IList<ConditionContainer>? Conditions { get; set; }
    public IList<MiddlewareContainer>? Middlewares { get; set; }

    public ValueTask<bool> CanHandleAsync(Update update, CancellationToken cancellationToken = default);
    public Task HandleAsync(Update update, CancellationToken cancellationToken = default);
}