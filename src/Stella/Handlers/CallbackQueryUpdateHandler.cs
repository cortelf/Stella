using Stella.Conditions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Stella.Handlers;

public abstract class CallbackQueryUpdateHandler: UpdateHandler
{
    public override void Configure()
    {
        UseCondition<MessageTypeCondition>(UpdateType.CallbackQuery);
    }

    public override Task HandleAsync(Update update, UpdateContext updateContext, CancellationToken cancellationToken = default)
    {
        return HandleAsync(update.CallbackQuery!, updateContext, cancellationToken);
    }

    public abstract Task HandleAsync(CallbackQuery callbackQuery, UpdateContext context,
        CancellationToken cancellationToken = default);
}
