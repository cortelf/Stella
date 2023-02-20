using Autofac;
using Stella;
using Stella.Example.Polling;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

var token = Environment.GetEnvironmentVariable("BOT_TOKEN")!;
var botClient = new TelegramBotClient(token);

var app = new StellaApp();

app.AddControllers();
app.DependencyInjection.RegisterInstance<ITelegramBotClient>(botClient);
app.Build();

var receiverOptions = new ReceiverOptions
{
    AllowedUpdates = Array.Empty<UpdateType>() // receive all update types
};
botClient.StartReceiving(
    updateHandler: new UpdateHandler(app: app),
    receiverOptions: receiverOptions
);
Console.ReadLine();
