using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;

namespace Stella.Middlewares;

internal class LoggingMiddlewareScope
{
    public long UpdateId { get; init; }
}

public class LoggingMiddleware(ILogger<LoggingMiddleware> logger): IMiddleware
{
    public async Task ProcessAsync(Update update, Func<Update, Task> next, CancellationToken cancellationToken = default)
    {
        using var scope = logger.BeginScope(new LoggingMiddlewareScope { UpdateId = update.Id });
        logger.LogInformation("Started processing Telegram update");
        await next(update);
        logger.LogInformation("Finished processing Telegram update");
    }
}