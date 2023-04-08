using Stella.Polling;

var token = Environment.GetEnvironmentVariable("BOT_TOKEN")!;
var app = new PollingApp(token);

app.AddBotControllers();
app.RunPolling();