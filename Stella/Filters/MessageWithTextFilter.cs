using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Stella.Filters;

public class MessageWithTextFilter : MessageTypeFilter
{
    public override bool Compare(Update update, IServiceProvider container)
    {
        return base.Compare(update, container) && update.Message!.Text != null;
    }
}