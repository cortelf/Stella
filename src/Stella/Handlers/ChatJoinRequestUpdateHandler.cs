using Stella.Conditions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Stella.Handlers;

public abstract class ChatJoinRequestUpdateHandler: UpdateHandler
{
    public override void Configure()
    {
        UseCondition<MessageTypeCondition>(UpdateType.ChatJoinRequest);
    }

    public override Task HandleAsync(Update update, UpdateContext updateContext, CancellationToken cancellationToken = default)
    {
        return HandleAsync(update.ChatJoinRequest!, updateContext, cancellationToken);
    }

    public abstract Task HandleAsync(ChatJoinRequest chatJoinRequest, UpdateContext context,
        CancellationToken cancellationToken = default);
}
