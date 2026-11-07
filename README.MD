# Stella – A Convenient Library for Writing Telegram Bots

![NuGet Version](https://img.shields.io/nuget/v/Stella?style=flat&label=Stella)
![NuGet Version](https://img.shields.io/nuget/v/Stella.AspNetCore?style=flat&label=Stella.AspNetCore)
![NuGet Version](https://img.shields.io/nuget/v/Stella.Polling?style=flat&label=Stella.Polling)
![GitHub License](https://img.shields.io/github/license/cortelf/Stella)

### Getting Started

##### Install the NuGet package

```powershell
dotnet add package Stella
```

##### Write your first handlers

Each handler is a class. It declares its conditions and middlewares in `Configure()` and does its work in `HandleAsync`.

```csharp
public class StartHandler(ITelegramBotClient bot, ILogger<StartHandler> logger) : MessageUpdateHandler
{
    public override void Configure()
    {
        base.Configure(); // keeps the "update is a Message" condition
        UseCondition<CommandCondition>("start");
    }

    public override async Task HandleAsync(Message message, UpdateContext context, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("/start in chat {ChatId}", message.Chat.Id);
        await bot.SendMessage(message.Chat.Id, "Hello from Stella!", cancellationToken: cancellationToken);
    }
}

public class HelpHandler(ITelegramBotClient bot) : MessageUpdateHandler
{
    public override void Configure()
    {
        base.Configure();
        UseCondition<CommandCondition>("help");
    }

    public override async Task HandleAsync(Message message, UpdateContext context, CancellationToken cancellationToken = default)
    {
        await bot.SendMessage(message.Chat.Id, "Do you need help?", cancellationToken: cancellationToken);
    }
}

public class EchoHandler(ITelegramBotClient bot) : MessageUpdateHandler
{
    public override void Configure()
    {
        base.Configure();
        SetPriority(100); // checked after the command handlers
    }

    public override async Task HandleAsync(Message message, UpdateContext context, CancellationToken cancellationToken = default)
    {
        if (message.Text is { } text)
            await bot.SendMessage(message.Chat.Id, text, cancellationToken: cancellationToken);
    }
}
```

How routing works:

- Handlers are checked in ascending priority order (default `0`), and the **first** handler whose conditions all pass handles the update. Give catch-all handlers a higher number.
- Handlers are resolved from DI as scoped services, so constructor injection works in `HandleAsync`.
- `Configure()` runs once at startup on an instance created **without calling its constructor**. Only call `UseCondition` / `UseMiddleware` / `SetPriority` there, and don't use injected dependencies or fields.

Base classes are available for every update type: `MessageUpdateHandler`, `EditedMessageUpdateHandler`, `CallbackQueryUpdateHandler`, `InlineQueryUpdateHandler`, `ChannelPostUpdateHandler`, `ChatMemberUpdateHandler`, `PreCheckoutQueryUpdateHandler` and so on. To work with the raw `Update`, derive from `UpdateHandler` directly.

##### Connect Stella to your application

Register the client for interacting with the Telegram API in Dependency Injection:

```csharp
builder.Services.AddSingleton<ITelegramBotClient>(
    new TelegramBotClient(builder.Configuration["BotToken"] ?? throw new ArgumentException("Token not found")));
```

Next, your steps will differ depending on whether you are using polling or a webhook.

For polling:

```powershell
dotnet add package Stella.Polling
```

```csharp
builder.Services.AddStellaPolling(builderAction: bld => bld
    .RegisterGlobalMiddleware<LoggingMiddleware>()
    .RegisterHandlersFromAssembly(Assembly.GetExecutingAssembly()));
```

For webhook:

```powershell
dotnet add package Stella.AspNetCore
```

```csharp
builder.Services.AddStella(bld => bld
    .RegisterGlobalMiddleware<LoggingMiddleware>()
    .RegisterHandlersFromAssembly(Assembly.GetExecutingAssembly()));
```

```csharp
app.UseStellaWebhookHandler("/webhook", exceptedSecretToken: builder.Configuration["WebhookSecret"]);
```

By default, Stella does not register your webhook with the Telegram API. You need to do this yourself, for example in a Hosted Service.

##### Polling options

```csharp
builder.Services.AddStellaPolling(
    cfgAction: cfg =>
    {
        cfg.AllowedUpdates = [UpdateType.Message, UpdateType.CallbackQuery];
        cfg.LongPollingTimeoutInSeconds = 30;      // how long Telegram holds getUpdates open
        cfg.ChatWorkerIdleTimeoutInSeconds = 60;   // idle time before a chat's worker exits
        cfg.PollingIntervalInMilliseconds = 500;   // retry delay after a failed getUpdates
    },
    builderAction: bld => bld.RegisterHandlersFromAssembly(Assembly.GetExecutingAssembly()));
```

Updates from the same chat are handled one at a time, in order. Different chats are handled concurrently.

### Middlewares

Middlewares wrap the handler call, similarly to ASP.NET Core. They are resolved from DI as scoped services.

```csharp
public class MyMiddleware(IMyDependency dep) : AsyncMiddleware
{
    public override async Task ProcessAsync(Update update, Func<Update, Task> next, CancellationToken cancellationToken = default)
    {
        // code before handler call
        await next(update);
        // code after handler call
    }
}
```

A middleware can also take a context value, given where it is attached:

```csharp
public class RequireRoleMiddleware(IRoleService roles) : AsyncMiddleware<string>
{
    public override async Task ProcessAsync(Update update, string role, Func<Update, Task> next, CancellationToken cancellationToken = default)
    {
        if (await roles.HasRoleAsync(update, role, cancellationToken))
            await next(update);
    }
}
```

Attach middlewares to a handler in `Configure()`:

```csharp
public override void Configure()
{
    base.Configure();
    UseMiddleware<MyMiddleware>();
    UseMiddleware<RequireRoleMiddleware, string>("admin");
}
```

##### Global Middlewares

Global middlewares run for every handled update:

```csharp
builder.Services.AddStellaPolling(builderAction: bld => bld   // or AddStella(...) for webhook
    .RegisterGlobalMiddleware<LoggingMiddleware>()
    .RegisterGlobalMiddleware<RequireRoleMiddleware, string>("user")
    .RegisterHandlersFromAssembly(Assembly.GetExecutingAssembly()));
```

Middlewares run in ascending `Priority` order: lower runs first (outermost). The built-in `LoggingMiddleware` uses `int.MinValue`, so it always wraps everything else.

### Conditions

Conditions decide whether a handler should process an update. They are resolved from DI as scoped services.

```csharp
public class PrivateChatCondition : AsyncCondition
{
    public override ValueTask<bool> CheckAsync(Update update, CancellationToken cancellationToken = default)
    {
        return ValueTask.FromResult(update.Message?.Chat.Type == ChatType.Private);
    }
}
```

With a context value:

```csharp
public class TextContainsCondition : AsyncCondition<string>
{
    public override ValueTask<bool> CheckAsync(Update update, string text, CancellationToken cancellationToken = default)
    {
        return ValueTask.FromResult(update.Message?.Text?.Contains(text) == true);
    }
}
```

Attach conditions to a handler in `Configure()`. All of them must pass:

```csharp
public override void Configure()
{
    base.Configure();
    UseCondition<PrivateChatCondition>();
    UseCondition<TextContainsCondition, string>("hello");
}
```

Built-in conditions:

- `MessageTypeCondition` checks the update type, e.g. `UseCondition<MessageTypeCondition, UpdateType>(UpdateType.Message)`. The typed base handlers add it for you.
- `CommandCondition` matches a bot command at the start of the message, case-insensitive, with or without the leading `/`: `UseCondition<CommandCondition>("start")`.
