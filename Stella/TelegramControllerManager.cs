using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Stella
{
    public class TelegramControllerManager : IControllerManager
    {
        private IControllerHandlersFetcher _controllerHandlersFetcher;
        public TelegramControllerManager(IControllerHandlersFetcher controllerHandlersFetcher) {
            _controllerHandlersFetcher = controllerHandlersFetcher;
        }

        private readonly IList<TelegramHandlerFilterData> _handlers = new List<TelegramHandlerFilterData>();
        public async Task ProcessUpdate(Update update, IContainer scope)
        {
            foreach (var handler in _handlers)
            {
                var canBeUsed = true;
                foreach (var filter in handler.Filters)
                {
                    if (!filter.Compare(update, scope))
                    {
                        canBeUsed = false;
                        break;
                    }
                }
                if (canBeUsed)
                {
                    await handler.Func(scope)(update);
                    break;
                }
            }
        }

        public void RegisterController(Type controller)
        {
            foreach (var data in _controllerHandlersFetcher.GetHandlers(controller))
            {
                _handlers.Add(data);
            }
        }
    }
}
