using Telegram.Bot.Types.Enums;

namespace Stella.Polling;

public class StellaPollingAppConfiguration
{
    /// <summary>
    /// Delay before retrying after a failed <c>getUpdates</c> call.
    /// Successful polls are not delayed: long polling already waits on the server.
    /// </summary>
    public int PollingIntervalInMilliseconds { get; set; } = 500;

    /// <summary>
    /// How long Telegram holds a <c>getUpdates</c> request open waiting for new updates (long polling).
    /// </summary>
    public int LongPollingTimeoutInSeconds { get; set; } = 30;

    /// <summary>
    /// How long a chat's worker waits for new updates before it exits. A new worker starts on the chat's next update.
    /// </summary>
    public int ChatWorkerIdleTimeoutInSeconds { get; set; } = 60;

    public UpdateType[]? AllowedUpdates { get; set; }
}
