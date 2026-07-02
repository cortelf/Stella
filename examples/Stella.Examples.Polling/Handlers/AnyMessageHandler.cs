using Stella.Handlers;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Stella.Examples.Polling.Handlers;

public class AnyMessageHandler(ILogger<AnyMessageHandler> logger, ITelegramBotClient bot): MessageUpdateHandler
{
    public override void Configure()
    {
        base.Configure();
        SetPriority(100);
    }
    public override Task HandleAsync(Message message, UpdateContext updateContext, CancellationToken cancellationToken = default)
    {
        return bot.SendMessage(message.From!.Id, message.Text!, cancellationToken: cancellationToken);
    }
}