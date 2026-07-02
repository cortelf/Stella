using Telegram.Bot.Types;

namespace Stella.Routing;

public interface IBotUpdateRouter
{
    Task RouteAsync(Update update, CancellationToken cancellationToken = default);
}