using Stella.Conditions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Stella.Handlers;

public abstract class PollUpdateHandler: UpdateHandler
{
    public override void Configure()
    {
        UseCondition<MessageTypeCondition>(UpdateType.Poll);
    }

    public override Task HandleAsync(Update update, UpdateContext updateContext, CancellationToken cancellationToken = default)
    {
        return HandleAsync(update.Poll!, updateContext, cancellationToken);
    }

    public abstract Task HandleAsync(Poll poll, UpdateContext context,
        CancellationToken cancellationToken = default);
}
