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
        private readonly IList<TelegramHandlerFilterData> _handlers = new List<TelegramHandlerFilterData>();
        public async Task ProcessUpdate(Update update, ITelegramHandlerScope scope)
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
                    await handler.Func(update, scope);
                    break;
                }
            }
        }

        public void RegisterController(ITelegramController controller)
        {
            foreach (var data in controller.GetHandlers())
            {
                _handlers.Add(data);
            }
        }
    }
}
