using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Stella
{
    public class TelegramHandlerFilterData
    {
        public Func<IServiceProvider, Func<Update, Task>> Func { get; set; } = null!;
        public List<ITelegramHandlerFilter> Filters { get; set; } = new List<ITelegramHandlerFilter>();
    }
}
