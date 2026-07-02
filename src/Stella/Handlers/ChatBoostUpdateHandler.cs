using Stella.Conditions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Stella.Handlers;

public abstract class ChatBoostUpdateHandler: UpdateHandler
{
    public override void Configure()
    {
        UseCondition<MessageTypeCondition>(UpdateType.ChatBoost);
    }

    public override Task HandleAsync(Update update, UpdateContext updateContext, CancellationToken cancellationToken = default)
    {
        return HandleAsync(update.ChatBoost!, updateContext, cancellationToken);
    }

    public abstract Task HandleAsync(ChatBoostUpdated chatBoost, UpdateContext context,
        CancellationToken cancellationToken = default);
}
