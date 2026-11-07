using Stella.Conditions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Stella.Handlers;

public abstract class PollAnswerUpdateHandler: UpdateHandler
{
    public override void Configure()
    {
        UseCondition<MessageTypeCondition>(UpdateType.PollAnswer);
    }

    public override Task HandleAsync(Update update, UpdateContext updateContext, CancellationToken cancellationToken = default)
    {
        return HandleAsync(update.PollAnswer!, updateContext, cancellationToken);
    }

    public abstract Task HandleAsync(PollAnswer pollAnswer, UpdateContext context,
        CancellationToken cancellationToken = default);
}
