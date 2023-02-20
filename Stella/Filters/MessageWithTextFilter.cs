using Autofac;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Stella.Filters;

public class MessageWithTextFilter : MessageTypeFilter
{
    public override bool Compare(Update update, IContainer container)
    {
        return base.Compare(update, container) && update.Message!.Text != null;
    }
}