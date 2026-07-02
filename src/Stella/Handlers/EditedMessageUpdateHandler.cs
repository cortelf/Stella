using Stella.Conditions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Stella.Handlers;

public abstract class EditedMessageUpdateHandler: UpdateHandler
{
    public override void Configure()
    {
        UseCondition<MessageTypeCondition>(UpdateType.EditedMessage);
    }

    public override Task HandleAsync(Update update, UpdateContext updateContext, CancellationToken cancellationToken = default)
    {
        return HandleAsync(update.EditedMessage!, updateContext, cancellationToken);
    }

    public abstract Task HandleAsync(Message message, UpdateContext context,
        CancellationToken cancellationToken = default);
}
