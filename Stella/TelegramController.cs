using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Stella
{
    public abstract class TelegramController : ITelegramController
    {
        public IList<TelegramHandlerFilterData> GetHandlers()
        {
            var result = new List<TelegramHandlerFilterData>();

            var classAttributes = GetType().GetCustomAttributes(typeof(FilterAttribute), true)
                .Select(x => (x as ITelegramHandlerFilter)!).ToList();
            var type = GetType();
            var methods = GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance);

            var validMethods = GetType().GetMethods().Where((m) =>
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
                    Delegate.CreateDelegate(typeof(Func<Update, ITelegramHandlerScope, Task>), this, methodInfo);

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
