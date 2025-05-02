using Telegram.Bot.Types.Enums;

namespace Stella.Polling;

public interface IStellaPollingApp
{
    Task RunPollingAsync(CancellationToken cancellationToken = default);
}