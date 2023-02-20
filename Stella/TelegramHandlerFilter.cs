using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Stella
{
    public abstract class TelegramHandlerFilter : ITelegramHandlerFilter
    {
        public TelegramHandlerFilterData FilterData { get; private set; }


        public TelegramHandlerFilter(TelegramHandlerFilter filter)
        {
            filter.FilterData.Filters.Insert(0, this);
            FilterData = filter.FilterData;
        }


        public abstract bool Compare(Update update, IContainer container);
    }
}
