using Telegram.Bot.Types;

namespace Stella.Conditions;

public abstract class AsyncCondition<TContext>: IAsyncCondition
{
    public int Priority { get; set; } = 0;
    
    public ValueTask<bool> CheckAsync(Update update, object? context, CancellationToken cancellationToken = default)
    {
        if (context is null)
            ArgumentNullException.ThrowIfNull(context);
        return CheckAsync(update, (TContext)context, cancellationToken);
    }

    public abstract ValueTask<bool> CheckAsync(Update update, TContext context, CancellationToken cancellationToken = default);
}

public abstract class AsyncCondition: IAsyncCondition
{
    public int Priority { get; set; } = 0;
    
    public ValueTask<bool> CheckAsync(Update update, object? context, CancellationToken cancellationToken = default)
    {
        return CheckAsync(update, cancellationToken);
    }

    public abstract ValueTask<bool> CheckAsync(Update update, CancellationToken cancellationToken = default);
}