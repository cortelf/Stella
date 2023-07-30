using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stella.Contracts;
using Telegram.Bot.Types;

namespace Stella
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public abstract class FilterAttribute : Attribute, ITelegramHandlerFilter
    {
        public abstract bool Compare(Update update, IServiceProvider container);
    }
}
