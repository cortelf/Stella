using System.Threading.Channels;
using Microsoft.Extensions.Logging;
using Stella.Routing;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Stella.Polling;

public class StellaPollingApp(
    IBotUpdateRouter router,
    ITelegramBotClient telegramBotClient,
    StellaPollingAppConfiguration configuration,
    ILogger<StellaPollingApp> logger) : IStellaPollingApp
{
    private sealed class ChatQueue
    {
        public Channel<Update> Updates { get; } =
            Channel.CreateUnbounded<Update>(new UnboundedChannelOptions { SingleReader = true });
        public Task Worker { get; set; } = Task.CompletedTask;
    }

    private int? _offset;

    private const long NoChatKey = 0;
    private readonly Dictionary<long, ChatQueue> _queues = new();

    public async Task RunPollingAsync(CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            Update[] updates;
            try
            {
                updates = await telegramBotClient.GetUpdates(
                    _offset,
                    timeout: configuration.LongPollingTimeoutInSeconds,
                    allowedUpdates: configuration.AllowedUpdates,
                    cancellationToken: cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception e)
            {
                logger.LogError(e, "An exception occurred while receiving updates");
                if (!await DelayAsync(configuration.PollingIntervalInMilliseconds, cancellationToken))
                    break;
                continue;
            }

            if (updates.Length == 0)
                continue;

            _offset = updates.Max(x => x.Id) + 1;
            foreach (var update in updates.OrderBy(x => x.Id))
                Enqueue(update, cancellationToken);
        }

        Task[] workers;
        lock (_queues)
            workers = _queues.Values.Select(x => x.Worker).ToArray();
        await Task.WhenAll(workers);
    }

    private void Enqueue(Update update, CancellationToken cancellationToken)
    {
        var key = GetChatKey(update);
        lock (_queues)
        {
            if (!_queues.TryGetValue(key, out var queue))
            {
                queue = new ChatQueue();
                _queues[key] = queue;
                queue.Worker = Task.Run(() => RunWorkerAsync(key, queue.Updates.Reader, cancellationToken),
                    CancellationToken.None);
            }

            queue.Updates.Writer.TryWrite(update);
        }
    }
    
    private async Task RunWorkerAsync(long key, ChannelReader<Update> reader, CancellationToken cancellationToken)
    {
        var idleTimeout = TimeSpan.FromSeconds(configuration.ChatWorkerIdleTimeoutInSeconds);
        while (true)
        {
            while (reader.TryRead(out var update))
                await HandleAsync(update, cancellationToken);

            using var idleCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            idleCts.CancelAfter(idleTimeout);
            try
            {
                await reader.WaitToReadAsync(idleCts.Token);
                continue;
            }
            catch (OperationCanceledException)
            {
            }

            lock (_queues)
            {
                if (reader.TryPeek(out _))
                    continue;
                _queues.Remove(key);
                return;
            }
        }
    }

    private async Task HandleAsync(Update update, CancellationToken cancellationToken)
    {
        try
        {
            await router.RouteAsync(update, cancellationToken);
        }
        catch (Exception e)
        {
            logger.LogError(e, "An exception occurred while handling update {UpdateId}", update.Id);
        }
    }

    private static long GetChatKey(Update update) =>
        update.Message?.Chat.Id
        ?? update.EditedMessage?.Chat.Id
        ?? update.ChannelPost?.Chat.Id
        ?? update.EditedChannelPost?.Chat.Id
        ?? update.BusinessMessage?.Chat.Id
        ?? update.EditedBusinessMessage?.Chat.Id
        ?? update.DeletedBusinessMessages?.Chat.Id
        ?? update.MessageReaction?.Chat.Id
        ?? update.MessageReactionCount?.Chat.Id
        ?? update.CallbackQuery?.Message?.Chat.Id
        ?? update.CallbackQuery?.From.Id
        ?? update.MyChatMember?.Chat.Id
        ?? update.ChatMember?.Chat.Id
        ?? update.ChatJoinRequest?.Chat.Id
        ?? update.ChatBoost?.Chat.Id
        ?? update.RemovedChatBoost?.Chat.Id
        ?? update.InlineQuery?.From.Id
        ?? update.ChosenInlineResult?.From.Id
        ?? update.ShippingQuery?.From.Id
        ?? update.PreCheckoutQuery?.From.Id
        ?? update.PurchasedPaidMedia?.From.Id
        ?? update.PollAnswer?.User?.Id
        ?? NoChatKey;

    private static async Task<bool> DelayAsync(int milliseconds, CancellationToken cancellationToken)
    {
        try
        {
            await Task.Delay(milliseconds, cancellationToken);
            return true;
        }
        catch (OperationCanceledException)
        {
            return false;
        }
    }
}
