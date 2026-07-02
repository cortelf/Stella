using Stella.Conditions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Stella.Handlers;

public abstract class EditedChannelPostUpdateHandler: UpdateHandler
{
    public override void Configure()
    {
        UseCondition<MessageTypeCondition>(UpdateType.EditedChannelPost);
    }

    public override Task HandleAsync(Update update, UpdateContext updateContext, CancellationToken cancellationToken = default)
    {
        return HandleAsync(update.EditedChannelPost!, updateContext, cancellationToken);
    }

    public abstract Task HandleAsync(Message channelPost, UpdateContext context,
        CancellationToken cancellationToken = default);
}
