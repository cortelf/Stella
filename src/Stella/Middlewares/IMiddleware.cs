using Telegram.Bot.Types;

namespace Stella.Middlewares;

public interface IMiddleware
{
    public int Priority { get; }
    Task ProcessAsync(Update update, Func<Update, Task> next, CancellationToken cancellationToken = default);
}