using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;

namespace Stella.Middlewares;

internal class LoggingMiddlewareScope
{
    public long UpdateId { get; init; }
}

public class LoggingMiddleware(ILogger<LoggingMiddleware> logger): AsyncMiddleware
{
    public override int Priority { get; set; } = int.MinValue;

    public override async Task ProcessAsync(Update update, Func<Update, Task> next, CancellationToken cancellationToken = default)
    {
        using var scope = logger.BeginScope(new LoggingMiddlewareScope { UpdateId = update.Id });
        logger.LogInformation("Started processing Telegram update");
        var startTimestamp = Stopwatch.GetTimestamp();
        try
        {
            await next(update);
        }
        finally
        {
            var elapsed = Stopwatch.GetElapsedTime(startTimestamp);
            logger.LogInformation("Finished processing Telegram update in {ElapsedMilliseconds} ms", elapsed.TotalMilliseconds);
        }
    }
}