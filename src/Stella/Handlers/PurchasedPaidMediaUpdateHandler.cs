using Stella.Conditions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.Payments;

namespace Stella.Handlers;

public abstract class PurchasedPaidMediaUpdateHandler: UpdateHandler
{
    public override void Configure()
    {
        UseCondition<MessageTypeCondition>(UpdateType.PurchasedPaidMedia);
    }

    public override Task HandleAsync(Update update, UpdateContext updateContext, CancellationToken cancellationToken = default)
    {
        return HandleAsync(update.PurchasedPaidMedia!, updateContext, cancellationToken);
    }

    public abstract Task HandleAsync(PaidMediaPurchased purchasedPaidMedia, UpdateContext context,
        CancellationToken cancellationToken = default);
}
