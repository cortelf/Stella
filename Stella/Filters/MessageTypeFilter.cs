using Autofac;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Stella.Filters;

public class MessageTypeFilter : FilterAttribute
{
    public override bool Compare(Update update, IContainer container)
    {
        return update.Type == UpdateType.Message && update.Message != null;
    }
}