using Stella.Conditions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Stella.Handlers;

public abstract class RemovedChatBoostUpdateHandler: UpdateHandler
{
    public override void Configure()
    {
        UseCondition<MessageTypeCondition>(UpdateType.RemovedChatBoost);
    }

    public override Task HandleAsync(Update update, UpdateContext updateContext, CancellationToken cancellationToken = default)
    {
        return HandleAsync(update.RemovedChatBoost!, updateContext, cancellationToken);
    }

    public abstract Task HandleAsync(ChatBoostRemoved removedChatBoost, UpdateContext context,
        CancellationToken cancellationToken = default);
}
