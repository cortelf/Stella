using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Stella.Contracts;
using Telegram.Bot.Types;

namespace Stella
{
    public class ControllerHandlersFetcher : IControllerHandlersFetcher
    {
        public IList<TelegramHandlerData> GetHandlers(Type type)
        {
            var result = new List<TelegramHandlerData>();

            var classFilterAttributes = type.GetCustomAttributes(typeof(FilterAttribute), true)
                .Select(x => (x as ITelegramHandlerFilter)!).ToList();
            var classMiddlewareAttributes = type.GetCustomAttributes(typeof(MiddlewareAttribute), true)
                .Select(x => (x as MiddlewareAttribute)!).ToList();
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);

            var validMethods = type.GetMethods().Where((m) =>
            {
                var parameters = m.GetParameters();
                if (parameters.Length != 1)
                {
                    return false;
                }
                if (parameters[0].ParameterType != typeof(Update))
                {
                    return false;
                }
                return true;
            }).ToList();

            foreach (var methodInfo in validMethods)
            {
                var func = (IServiceProvider b) => (Func<Update, Task>)
                    Delegate.CreateDelegate(typeof(Func<Update, Task>), b.GetService(type), methodInfo);

                var methodFilterAttributes = methodInfo.GetCustomAttributes(typeof(FilterAttribute), true)
                    .Select(x => (x as ITelegramHandlerFilter)!).ToList();
                var methodMiddlewareAttributes = methodInfo.GetCustomAttributes(typeof(MiddlewareAttribute), true)
                    .Select(x => (x as MiddlewareAttribute)!).ToList();

                var data = new TelegramHandlerData()
                {
                    Func = func,
                    Filters = classFilterAttributes.Concat(methodFilterAttributes).ToList(),
                    Middlewares = classMiddlewareAttributes.Concat(methodMiddlewareAttributes).ToList()
                };
                result.Add(data);
            }

            return result;
        }
    }
}
