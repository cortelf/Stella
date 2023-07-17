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
        private readonly IControllerHandlersFetcher _controllerHandlersFetcher;
        private readonly IFilterResolver _filterResolver;
        public TelegramControllerManager(IControllerHandlersFetcher controllerHandlersFetcher, IFilterResolver filterResolver)
        {
            _controllerHandlersFetcher = controllerHandlersFetcher;
            _filterResolver = filterResolver;
        }

        private readonly IList<TelegramHandlerData> _handlers = new List<TelegramHandlerData>();
        public async Task ProcessUpdate(Update update, IServiceProvider scope)
        {
            var handlerAfterResolveFilters = _filterResolver.Resolve(_handlers, update, scope);
            if (handlerAfterResolveFilters == null) return;
            var lastHandler = handlerAfterResolveFilters.Func(scope);
            IList<Func<Update, Task>> completedMiddlewares = new List<Func<Update, Task>> { lastHandler };
            foreach (var middleware in handlerAfterResolveFilters.Middlewares.Reverse())
            {
                var previousMiddleware = completedMiddlewares.First();
                completedMiddlewares.Insert(0, (Update u) => middleware.Process(update, scope, previousMiddleware));
            }

            await completedMiddlewares.First()(update);
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
