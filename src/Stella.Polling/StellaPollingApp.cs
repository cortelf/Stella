using Microsoft.Extensions.Logging;
using Stella.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Stella.Polling;

public class StellaPollingApp(
    IBotUpdateRouter router,
    ITelegramBotClient telegramBotClient,
    StellaPollingAppConfiguration configuration,
    ILogger<StellaPollingApp> logger) : IStellaPollingApp
{
    private int? _offset;

    public async Task RunPollingAsync(CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var updates = await telegramBotClient.GetUpdates(_offset, allowedUpdates: configuration.AllowedUpdates);
                if (updates.Length > 0)
                    _offset = updates.Last().Id + 1;

                var tasks = updates.Select(x => router.RouteAsync(x));
                await Task.WhenAll(tasks);
            }
            catch (Exception e)
            {
                logger.LogError(e, "An exception occured while handling a message");
            }

            try
            {
                await Task.Delay(configuration.PollingIntervalInMilliseconds, cancellationToken);
            }
            catch (TaskCanceledException)
            {
                break;
            }
        }

    }
}