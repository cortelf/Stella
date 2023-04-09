using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;

namespace Stella.Middlewares;

public class LoggingMiddlewareAttribute: MiddlewareAttribute
{
    public override async Task Process(Update update, IServiceProvider container, Func<Update, Task> next)
    {
        var logger  = (container.GetService(typeof(ILogger<LoggingMiddlewareAttribute>)) as ILogger<LoggingMiddlewareAttribute>)!;
        logger.LogInformation("New update {UpdateId}", update.Id);
        var stopwatch = Stopwatch.StartNew();
        await next(update);
        stopwatch.Stop();
        logger.LogInformation("Update {UpdateId} execution time {ElapsedMilliseconds} ms.", update.Id, stopwatch.ElapsedMilliseconds);

    }
}