using Telegram.Bot.Types;

namespace Stella.Middlewares;

public abstract class AsyncMiddleware<TContext>: IAsyncMiddleware
{
    public int Priority { get; set; } = 0;
    
    public Task ProcessAsync(Update update, object? context, Func<Update, Task> next, CancellationToken cancellationToken = default)
    {
        if (context is null)
            ArgumentNullException.ThrowIfNull(context);
        return ProcessAsync(update, (TContext)context, next, cancellationToken);
    }

    public abstract Task ProcessAsync(Update update, TContext context, Func<Update, Task> next,
        CancellationToken cancellationToken = default);
}

public abstract class AsyncMiddleware: IAsyncMiddleware
{
    public virtual int Priority { get; set; } = 0;
    
    public Task ProcessAsync(Update update, object? context, Func<Update, Task> next, CancellationToken cancellationToken = default)
    {
        return ProcessAsync(update, next, cancellationToken);
    }

    public abstract Task ProcessAsync(Update update, Func<Update, Task> next,
        CancellationToken cancellationToken = default);
}