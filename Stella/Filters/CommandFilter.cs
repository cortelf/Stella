using Telegram.Bot.Types;

namespace Stella.Filters;

public class CommandFilter : MessageWithTextFilter
{
    private readonly List<string> _commands;
    public CommandFilter(List<string> allowedCommand)
    {
        _commands = allowedCommand;
    }
    public CommandFilter(string command)
    {
        _commands = new List<string>() {command};
    }
    public override bool Compare(Update update, ITelegramHandlerScope scope)
    {
        return base.Compare(update, scope) && _commands.Any(command => update.Message!.Text!.StartsWith(command));
    }
}