using Stella.Filters;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Stella.Example.Polling;

[BotController]
public class SampleController
{
    [CommandFilter("/start")]
    public async Task OnStart(Update update, ITelegramHandlerScope scope)
    {
        var bot = scope.Get<ITelegramBotClient>();
        await bot!.SendTextMessageAsync(update.Message!.From!.Id, "Hello from Stella!");
    }

    [CommandFilter("/help")]
    public async Task OnHelp(Update update, ITelegramHandlerScope scope)
    {
        var bot = scope.Get<ITelegramBotClient>();
        await bot!.SendTextMessageAsync(update.Message!.From!.Id, "Do you need help?");
    }

    public async Task OnAnyMessage(Update update, ITelegramHandlerScope scope)
    {
        var bot = scope.Get<ITelegramBotClient>();
        await bot!.SendTextMessageAsync(update.Message!.From!.Id, update.Message!.Text!);
    }
}