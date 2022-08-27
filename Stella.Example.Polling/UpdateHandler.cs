using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace Stella.Example.Polling;

public class UpdateHandler : IUpdateHandler
{
    private readonly IControllerManager _controllerManager;
    public UpdateHandler(IControllerManager controllerManager)
    {
        _controllerManager = controllerManager;
    }
    
    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        var scope = new HandlerScope();
        scope.Add(botClient);

        await _controllerManager.ProcessUpdate(update, scope);
    }

    public async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}