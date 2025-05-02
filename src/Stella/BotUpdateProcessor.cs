using System.Reflection;
using Stella.Interfaces;
using Stella.Middlewares;
using Telegram.Bot.Types;

namespace Stella;

public class BotUpdateProcessor: IBotUpdateProcessor
{
    public async Task ProcessAsync(Update update, MethodInfo methodInfo, object controllerInstance, IReadOnlyList<IMiddleware> middlewares,
        CancellationToken cancellationToken = default)
    {
        var parameters = methodInfo.GetParameters();
        var ctNeeded = parameters[1].ParameterType == typeof(CancellationToken);

        List<object> methodArgs = [update];
        if (ctNeeded)
            methodArgs.Add(cancellationToken);

        Func<Update, Task> methodFunc = upd => (Task)methodInfo.Invoke(controllerInstance, methodArgs.ToArray())!;
        var funcMiddleware = middlewares
            .Reverse()
            .Aggregate(methodFunc, (func, middleware) => upd => middleware.ProcessAsync(upd, func, cancellationToken));


        await funcMiddleware.Invoke(update);
    }
}