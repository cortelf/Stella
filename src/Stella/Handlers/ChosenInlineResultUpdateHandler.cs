using Stella.Conditions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Stella.Handlers;

public abstract class ChosenInlineResultUpdateHandler: UpdateHandler
{
    public override void Configure()
    {
        UseCondition<MessageTypeCondition>(UpdateType.ChosenInlineResult);
    }

    public override Task HandleAsync(Update update, UpdateContext updateContext, CancellationToken cancellationToken = default)
    {
        return HandleAsync(update.ChosenInlineResult!, updateContext, cancellationToken);
    }

    public abstract Task HandleAsync(ChosenInlineResult chosenInlineResult, UpdateContext context,
        CancellationToken cancellationToken = default);
}
