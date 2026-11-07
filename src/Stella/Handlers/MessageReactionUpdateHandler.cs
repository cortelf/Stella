using Stella.Conditions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Stella.Handlers;

public abstract class MessageReactionUpdateHandler: UpdateHandler
{
    public override void Configure()
    {
        UseCondition<MessageTypeCondition>(UpdateType.MessageReaction);
    }

    public override Task HandleAsync(Update update, UpdateContext updateContext, CancellationToken cancellationToken = default)
    {
        return HandleAsync(update.MessageReaction!, updateContext, cancellationToken);
    }

    public abstract Task HandleAsync(MessageReactionUpdated messageReaction, UpdateContext context,
        CancellationToken cancellationToken = default);
}
