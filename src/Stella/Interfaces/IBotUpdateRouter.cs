using Telegram.Bot.Types;

namespace Stella.Interfaces;

public interface IBotUpdateRouter
{
    Task RouteAsync(Update update, CancellationToken cancellationToken = default);
}