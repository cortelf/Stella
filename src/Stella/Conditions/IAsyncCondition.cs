using Telegram.Bot.Types;

namespace Stella.Conditions;

public interface IAsyncCondition
{
    public int Priority { get; }
    ValueTask<bool> CheckAsync(Update update, object? context, CancellationToken cancellationToken = default);
}

