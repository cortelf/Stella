using Microsoft.Extensions.Logging;
using Stella.Filters;
using Stella.Middlewares;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Stella.Example.AspNetCore;

[BotController]
[LoggingMiddleware]
public class SampleController
{
    private ITelegramBotClient _bot;
    private ILogger<SampleController> _logger;

    public SampleController(ITelegramBotClient bot, ILogger<SampleController> logger)
    {
        _bot = bot;
        _logger = logger;
    }

    [CommandFilter("/start")]
    public async Task OnStart(Update update)
    {
        _logger.LogInformation("/start from {UserId}", update.Message!.From!.Id);
        await _bot.SendTextMessageAsync(update.Message!.From!.Id, "Hello from Stella!");
    }

    [CommandFilter("/help")]
    public async Task OnHelp(Update update)
    {
        await _bot.SendTextMessageAsync(update.Message!.From!.Id, "Do you need help?");
    }

    public async Task OnAnyMessage(Update update)
    {
        await _bot.SendTextMessageAsync(update.Message!.From!.Id, update.Message!.Text!);
    }
}