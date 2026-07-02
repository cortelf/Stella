using Stella.Conditions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.Payments;

namespace Stella.Handlers;

public abstract class PreCheckoutQueryUpdateHandler: UpdateHandler
{
    public override void Configure()
    {
        UseCondition<MessageTypeCondition>(UpdateType.PreCheckoutQuery);
    }

    public override Task HandleAsync(Update update, UpdateContext updateContext, CancellationToken cancellationToken = default)
    {
        return HandleAsync(update.PreCheckoutQuery!, updateContext, cancellationToken);
    }

    public abstract Task HandleAsync(PreCheckoutQuery preCheckoutQuery, UpdateContext context,
        CancellationToken cancellationToken = default);
}
