using Telegram.Bot.Types;

namespace Stella;

public interface IFilterResolver
{
    TelegramHandlerData? Resolve(
        IEnumerable<TelegramHandlerData> filterData, 
        Update update, IServiceProvider provider);
}