using Stella.Filters;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Stella.Example.Polling;

[BotController]
public class SampleController
{
    private ITelegramBotClient _bot;

    public SampleController(ITelegramBotClient bot)
    {
        _bot = bot;
    }

    [CommandFilter("/start")]
    public async Task OnStart(Update update)
    {
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