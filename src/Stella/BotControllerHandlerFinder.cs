using System.Reflection;
using Stella.Interfaces;
using Telegram.Bot.Types;

namespace Stella;

public class BotControllerHandlerFinder: IBotControllerHandlerFinder
{
    public IReadOnlyList<MethodInfo> Find(Type type)
    {
        return type.GetMethods().Where(x =>
        {
            var parameters = x.GetParameters();
            if (parameters.Length is > 2 or < 1)
            {
                return false;
            }

            if (parameters[0].ParameterType != typeof(Update))
            {
                return false;
            }

            if (parameters.Length > 1 && parameters[1].ParameterType != typeof(CancellationToken))
            {
                return false;
            }

            if (x.ReturnType != typeof(Task))
            {
                return false;
            }

            return true;
        }).ToList();
    }
}