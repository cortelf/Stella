using Stella.Polling;
using Telegram.Bot;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddSingleton<ITelegramBotClient>(
    new TelegramBotClient(builder.Configuration["BotToken"] ?? throw new ArgumentException("Token not found")));
builder.Services.AddStellaPolling();

var host = builder.Build();
host.Run();