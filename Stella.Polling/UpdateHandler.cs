using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace Stella.Polling;

public class UpdateHandler : IUpdateHandler
{
    private readonly PollingApp _app;
    private readonly IServiceProvider _serviceProvider;
    
    public UpdateHandler(PollingApp app, IServiceProvider serviceProvider)
    {
        _app = app;
        _serviceProvider = serviceProvider;
    }
    
    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        await _app.ControllerManager.ProcessUpdate(update, _serviceProvider);
    }

    public async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}