using Stella;
using Stella.Example.Polling;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

var token = Environment.GetEnvironmentVariable("BOT_TOKEN")!;
var botClient = new TelegramBotClient(token);

var controllerManager = new TelegramControllerManager();
controllerManager.RegisterController(new SampleController());

var receiverOptions = new ReceiverOptions
{
    AllowedUpdates = Array.Empty<UpdateType>() // receive all update types
};
botClient.StartReceiving(
    updateHandler: new UpdateHandler(controllerManager: controllerManager),
    receiverOptions: receiverOptions
);
Console.ReadLine();
