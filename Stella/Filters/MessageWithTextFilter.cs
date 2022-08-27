using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Stella.Filters;

public class MessageWithTextFilter : MessageTypeFilter
{
    public override bool Compare(Update update, ITelegramHandlerScope scope)
    {
        return base.Compare(update, scope) && update.Message!.Text != null;
    }
}