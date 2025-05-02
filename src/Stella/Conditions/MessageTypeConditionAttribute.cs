using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Stella.Conditions;

public class MessageTypeConditionAttribute(UpdateType updateType)
    : ConditionAttribute<MessageTypeConditionAttribute>, ICondition
{
    public virtual bool Check(Update update)
    {
        return update.Type == updateType;
    }
}