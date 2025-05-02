using Telegram.Bot.Types;

namespace Stella.Middlewares;

public interface IMiddleware
{ 
    Task ProcessAsync(Update update, Func<Update, Task> next, CancellationToken cancellationToken = default);
}