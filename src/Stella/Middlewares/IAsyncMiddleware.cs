using Telegram.Bot.Types;

namespace Stella.Middlewares;

public interface IAsyncMiddleware
{
    public int Priority { get; }
    Task ProcessAsync(Update update, object? context, Func<Update, Task> next, CancellationToken cancellationToken = default);
}