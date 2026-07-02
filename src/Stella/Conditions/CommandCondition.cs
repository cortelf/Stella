using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Stella.Conditions;

public class CommandCondition : AsyncCondition<string>
{
    public override ValueTask<bool> CheckAsync(Update update, string context, CancellationToken cancellationToken = default)
    {
        if (update.Message is not { Text: { } text, Entities: { } entities })
            return ValueTask.FromResult(false);

        var entity = entities.FirstOrDefault(x => x is { Type: MessageEntityType.BotCommand, Offset: 0 });
        if (entity is null)
            return ValueTask.FromResult(false);

        var command = text.AsSpan(entity.Offset + 1, entity.Length - 1);
        var at = command.IndexOf('@');
        if (at >= 0)
            command = command[..at];

        var expected = context.AsSpan();
        if (!expected.IsEmpty && expected[0] == '/')
            expected = expected[1..];

        return ValueTask.FromResult(command.Equals(expected, StringComparison.OrdinalIgnoreCase));
    }
}