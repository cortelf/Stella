using Stella.Conditions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Stella.Handlers;

public abstract class MyChatMemberUpdateHandler: UpdateHandler
{
    public override void Configure()
    {
        UseCondition<MessageTypeCondition>(UpdateType.MyChatMember);
    }

    public override Task HandleAsync(Update update, UpdateContext updateContext, CancellationToken cancellationToken = default)
    {
        return HandleAsync(update.MyChatMember!, updateContext, cancellationToken);
    }

    public abstract Task HandleAsync(ChatMemberUpdated myChatMember, UpdateContext context,
        CancellationToken cancellationToken = default);
}
