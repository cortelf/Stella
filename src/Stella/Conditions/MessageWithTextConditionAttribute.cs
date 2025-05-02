using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Stella.Conditions;

public class MessageWithTextConditionAttribute() : MessageTypeConditionAttribute(UpdateType.Message)
{
    public override bool Check(Update update)
    {
        return base.Check(update) && update.Message!.Text != null;
    }
}