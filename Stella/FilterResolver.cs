using Telegram.Bot.Types;

namespace Stella;

public class FilterResolver : IFilterResolver
{
    public TelegramHandlerData? Resolve(IEnumerable<TelegramHandlerData> filterData, Update update, IServiceProvider provider)
    {
        foreach (var handler in filterData)
        {
            var canBeUsed = handler.Filters.All(filter => filter.Compare(update, provider));
            if (canBeUsed)
            {
                return handler;
            }
        }

        return null;
    }
}