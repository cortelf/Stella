using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Stella
{
    public class TelegramHandlerData
    {
        public Func<IServiceProvider, Func<Update, Task>> Func { get; set; } = null!;
        public IList<ITelegramHandlerFilter> Filters { get; set; } = new List<ITelegramHandlerFilter>();
        public IList<MiddlewareAttribute> Middlewares { get; set; } = new List<MiddlewareAttribute>();
    }
}
