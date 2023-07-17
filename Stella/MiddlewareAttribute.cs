using Telegram.Bot.Types;

namespace Stella;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public abstract class MiddlewareAttribute: Attribute
{
    public abstract Task Process(Update update, IServiceProvider container, Func<Update, Task> next);
}