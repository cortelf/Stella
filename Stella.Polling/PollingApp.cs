using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace Stella.Polling;

public class PollingApp: IPollingApp
{
    private ITelegramBotClient _client;
    public IServiceCollection Services { get; }
    public IControllerManager ControllerManager { get; }
    public IControllersFetcher ControllersFetcher { get; }
    public ReceiverOptions? ReceiverOptions { get; set; }
    public ILogger SerilogLogger { get; set; }
    public PollingApp(string token)
    {
        _client = new TelegramBotClient(token);
        
        Services = new ServiceCollection();
        
        Services.AddSingleton<ITelegramBotClient>(_client);
        Services.AddSingleton<PollingApp>(this);
        Services.AddSingleton<IUpdateHandler, UpdateHandler>();
        
        ControllerManager = new TelegramControllerManager(new ControllerHandlersFetcher(), new FilterResolver());
        ControllersFetcher =  new ControllersFetcher();
        SerilogLogger = new LoggerConfiguration().MinimumLevel.Information().WriteTo.Console().CreateLogger();
    }
    public void RunPolling(CancellationToken cancellationToken = default)
    {
        var provider = Services
            .AddLogging(x => x.AddSerilog(SerilogLogger))
            .BuildServiceProvider();
        _client.StartReceiving(provider.GetService<IUpdateHandler>()!, ReceiverOptions, cancellationToken);

        var me = _client.GetMeAsync(cancellationToken).Result;
        SerilogLogger.Information("Bot {BotUsername} with id {BotId} is running", me.Username, me.Id);

        var thread = new Thread(
            () => {while (true) Thread.Sleep(5000);}
        );
        thread.Start();
    }
}