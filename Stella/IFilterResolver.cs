using Telegram.Bot.Types;

namespace Stella;

public interface IFilterResolver
{
    Func<IServiceProvider, Func<Update, Task>>? Resolve(
        IEnumerable<TelegramHandlerFilterData> filterData, 
        Update update, IServiceProvider provider);
}