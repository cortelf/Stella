using Stella.Conditions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Stella.Handlers;

public abstract class ChannelPostUpdateHandler: UpdateHandler
{
    public override void Configure()
    {
        UseCondition<MessageTypeCondition>(UpdateType.ChannelPost);
    }

    public override Task HandleAsync(Update update, UpdateContext updateContext, CancellationToken cancellationToken = default)
    {
        return HandleAsync(update.ChannelPost!, updateContext, cancellationToken);
    }

    public abstract Task HandleAsync(Message channelPost, UpdateContext context,
        CancellationToken cancellationToken = default);
}
