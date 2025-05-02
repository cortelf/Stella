using Telegram.Bot.Types;

namespace Stella.Conditions;

public interface ICondition
{
    bool Check(Update update);
}