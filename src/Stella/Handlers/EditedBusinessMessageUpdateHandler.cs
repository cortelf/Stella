using Stella.Conditions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Stella.Handlers;

public abstract class EditedBusinessMessageUpdateHandler: UpdateHandler
{
    public override void Configure()
    {
        UseCondition<MessageTypeCondition>(UpdateType.EditedBusinessMessage);
    }

    public override Task HandleAsync(Update update, UpdateContext updateContext, CancellationToken cancellationToken = default)
    {
        return HandleAsync(update.EditedBusinessMessage!, updateContext, cancellationToken);
    }

    public abstract Task HandleAsync(Message message, UpdateContext context,
        CancellationToken cancellationToken = default);
}
