using System.Reflection;
using Stella.Extensions;
using Stella.Middlewares;
using Stella.Polling;
using Telegram.Bot;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddSingleton<ITelegramBotClient>(
    new TelegramBotClient(builder.Configuration["BotToken"] ?? throw new ArgumentException("Token not found")));
builder.Services.AddStellaPolling(builderAction: configurationBuilder => 
    configurationBuilder
        .RegisterGlobalMiddleware<LoggingMiddleware>()
        .RegisterHandlersFromAssembly(Assembly.GetExecutingAssembly()));

var host = builder.Build();
host.Run();