using Stella.Conditions;
using Stella.Middlewares;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Stella.Examples.Webhook;

[BotController]
[Middleware<LoggingMiddleware>]
public class SampleController(ITelegramBotClient bot, ILogger<SampleController> logger)
{
    [CommandCondition("/start")]
    public async Task OnStart(Update update, CancellationToken cancellationToken)
    {
        logger.LogInformation("/start from {UserId}", update.Message!.From!.Id);
        await bot.SendMessage(update.Message!.From!.Id, "Hello from Stella!", cancellationToken: cancellationToken);
    }

    [CommandCondition("/help")]
    public async Task OnHelp(Update update, CancellationToken cancellationToken)
    {
        await bot.SendMessage(update.Message!.From!.Id, "Do you need help?", cancellationToken: cancellationToken);
    }

    public async Task OnAnyMessage(Update update, CancellationToken cancellationToken)
    {
        await bot.SendMessage(update.Message!.From!.Id, update.Message!.Text!, cancellationToken: cancellationToken);
    }
}