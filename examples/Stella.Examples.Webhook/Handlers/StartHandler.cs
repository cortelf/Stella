using Stella.Conditions;
using Stella.Handlers;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Stella.Examples.Webhook.Handlers;

public class StartHandler(ILogger<StartHandler> logger, ITelegramBotClient bot): MessageUpdateHandler
{
    public override void Configure()
    {
        base.Configure();
        UseCondition<CommandCondition>("start");
    }

    public override Task HandleAsync(Message message, UpdateContext context, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("/start from {UserId}", message.From!.Id);
        return bot.SendMessage(message.From!.Id, "Hello from Stella!", cancellationToken: cancellationToken);
    }
}