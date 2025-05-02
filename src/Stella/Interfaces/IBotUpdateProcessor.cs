using System.Reflection;
using Stella.Middlewares;
using Telegram.Bot.Types;

namespace Stella.Interfaces;

public interface IBotUpdateProcessor
{
    Task ProcessAsync(Update update, MethodInfo methodInfo, object controllerInstance, IReadOnlyList<IMiddleware> middlewares,
        CancellationToken cancellationToken = default);
}