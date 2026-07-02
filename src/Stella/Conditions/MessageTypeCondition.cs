using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Stella.Conditions;

public class MessageTypeCondition: AsyncCondition<UpdateType>
{
    public override ValueTask<bool> CheckAsync(Update update, UpdateType context, CancellationToken cancellationToken = default)
    {
        return ValueTask.FromResult(context == update.Type);
    }
}