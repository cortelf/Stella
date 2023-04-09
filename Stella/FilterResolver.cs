using Telegram.Bot.Types;

namespace Stella;

public class FilterResolver : IFilterResolver
{
    public Func<IServiceProvider, Func<Update, Task>>? Resolve(IEnumerable<TelegramHandlerFilterData> filterData, Update update, IServiceProvider provider)
    {
        foreach (var handler in filterData)
        {
            var canBeUsed = handler.Filters.All(filter => filter.Compare(update, provider));
            if (canBeUsed)
            {
                return handler.Func;
            }
        }

        return null;
    }
}