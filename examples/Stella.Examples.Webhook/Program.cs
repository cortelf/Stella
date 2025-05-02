using Stella.AspNetCore;
using Telegram.Bot;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<ITelegramBotClient>(
    new TelegramBotClient(builder.Configuration["BotToken"] ?? throw new ArgumentException("Token not found")));
builder.Services.AddStella();

var app = builder.Build();
app.UseStellaWebhookHandler("/webhook");
app.Run();