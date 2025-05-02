using Telegram.Bot.Types;

namespace Stella.Conditions;

public class CommandConditionAttribute(List<string> allowedCommand) : MessageWithTextConditionAttribute
{
    public CommandConditionAttribute(string command) : this([command])
    {
    }

    public override bool Check(Update update)
    {
        return base.Check(update) && allowedCommand.Any(command => update.Message!.Text!.StartsWith(command));
    }
}