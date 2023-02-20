using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace Stella.Example.Polling;

public class UpdateHandler : IUpdateHandler
{
    private readonly StellaApp _app;
    public UpdateHandler(StellaApp app)
    {
        _app = app;
    }
    
    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        await _app.ProcessUpdate(update);
    }

    public async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}