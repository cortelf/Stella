using Stella.Conditions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Stella.Handlers;

public abstract class MessageReactionCountUpdateHandler: UpdateHandler
{
    public override void Configure()
    {
        UseCondition<MessageTypeCondition>(UpdateType.MessageReactionCount);
    }

    public override Task HandleAsync(Update update, UpdateContext updateContext, CancellationToken cancellationToken = default)
    {
        return HandleAsync(update.MessageReactionCount!, updateContext, cancellationToken);
    }

    public abstract Task HandleAsync(MessageReactionCountUpdated reactionCount, UpdateContext context,
        CancellationToken cancellationToken = default);
}
