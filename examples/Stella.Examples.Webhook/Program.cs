using System.Reflection;
using Stella.AspNetCore;
using Stella.Extensions;
using Stella.Middlewares;
using Telegram.Bot;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<ITelegramBotClient>(
    new TelegramBotClient(builder.Configuration["BotToken"] ?? throw new ArgumentException("Token not found")));
builder.Services.AddStella(configurationBuilder => configurationBuilder
    .RegisterGlobalMiddleware<LoggingMiddleware>()
    .RegisterHandlersFromAssembly(Assembly.GetExecutingAssembly()));

var app = builder.Build();
app.UseStellaWebhookHandler("/webhook");
app.Run();