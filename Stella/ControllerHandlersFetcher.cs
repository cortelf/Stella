using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Stella
{
    public class ControllerHandlersFetcher : IControllerHandlersFetcher
    {
        public IList<TelegramHandlerFilterData> GetHandlers(Type type)
        {
            var result = new List<TelegramHandlerFilterData>();

            var classAttributes = type.GetCustomAttributes(typeof(FilterAttribute), true)
                .Select(x => (x as ITelegramHandlerFilter)!).ToList();
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);

            var validMethods = type.GetMethods().Where((m) =>
            {
                var parameters = m.GetParameters();
                if (parameters.Length != 2)
                {
                    return false;
                }
                if (parameters[0].ParameterType != typeof(Update))
                {
                    return false;
                }
                if (parameters[1].ParameterType != typeof(ITelegramHandlerScope))
                {
                    return false;
                }
                return true;
            }).ToList();

            foreach (var methodInfo in validMethods)
            {
                var func = (Func<Update, ITelegramHandlerScope, Task>)
                    Delegate.CreateDelegate(typeof(Func<Update, ITelegramHandlerScope, Task>), Activator.CreateInstance(type), methodInfo);

                var filterAttributes = methodInfo.GetCustomAttributes(typeof(FilterAttribute), true).Select(x => (x as ITelegramHandlerFilter)!).ToList();

                var data = new TelegramHandlerFilterData()
                {
                    Func = func,
                    Filters = classAttributes.Concat(filterAttributes).ToList()
                };
                result.Add(data);
            }

            return result;
        }
    }
}
