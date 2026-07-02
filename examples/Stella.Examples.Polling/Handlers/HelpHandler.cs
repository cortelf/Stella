using Stella.Conditions;
using Stella.Handlers;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Stella.Examples.Polling.Handlers;

public class HelpHandler(ILogger<HelpHandler> logger, ITelegramBotClient bot): MessageUpdateHandler
{
    public override void Configure()
    {
        base.Configure();
        UseCondition<CommandCondition>("help");
    }

    public override Task HandleAsync(Message message, UpdateContext context, CancellationToken cancellationToken = default)
    {
        return bot.SendMessage(message.From!.Id, "Do you need help?", cancellationToken: cancellationToken);
    }
}