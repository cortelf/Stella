using Telegram.Bot.Types.Enums;

namespace Stella.Polling;

public class StellaPollingAppConfiguration
{
    public int PollingIntervalInMilliseconds { get; set; } = 500;
    public UpdateType[]? AllowedUpdates { get; set; }
}