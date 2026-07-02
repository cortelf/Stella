using Stella.Conditions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.Payments;

namespace Stella.Handlers;

public abstract class ShippingQueryUpdateHandler: UpdateHandler
{
    public override void Configure()
    {
        UseCondition<MessageTypeCondition>(UpdateType.ShippingQuery);
    }

    public override Task HandleAsync(Update update, UpdateContext updateContext, CancellationToken cancellationToken = default)
    {
        return HandleAsync(update.ShippingQuery!, updateContext, cancellationToken);
    }

    public abstract Task HandleAsync(ShippingQuery shippingQuery, UpdateContext context,
        CancellationToken cancellationToken = default);
}
