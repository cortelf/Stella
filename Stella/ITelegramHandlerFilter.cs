using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Stella
{
    public interface ITelegramHandlerFilter
    {
        bool Compare(Update update, ITelegramHandlerScope scope);
    }
}
