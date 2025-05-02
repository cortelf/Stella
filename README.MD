# Stella – A Convenient Library for Writing Telegram Bots in MVC Style

### Getting Started

##### Install the NuGet package

```powershell
dotnet add package Stella
```

##### Write your first controller

```csharp
[BotController]  
[Middleware<LoggingMiddleware>]  
public class SampleController(ITelegramBotClient bot, ILogger<SampleController> logger)  
{  
    [CommandCondition("/start")]  
    public async Task OnStart(Update update, CancellationToken cancellationToken)  
    {  
        logger.LogInformation("/start from {UserId}", update.Message!.From!.Id);  
        await bot.SendMessage(update.Message!.From!.Id, "Hello from Stella!", cancellationToken: cancellationToken);  
    }  
  
    [CommandCondition("/help")]  
    public async Task OnHelp(Update update, CancellationToken cancellationToken)  
    {  
        await bot.SendMessage(update.Message!.From!.Id, "Do you need help?", cancellationToken: cancellationToken);  
    }  
  
    public async Task OnAnyMessage(Update update, CancellationToken cancellationToken)  
    {  
        await bot.SendMessage(update.Message!.From!.Id, update.Message!.Text!, cancellationToken: cancellationToken);  
    }  
}
```

Make sure to annotate your controllers with the `BotController` attribute, otherwise the library won't be able to find them.

In this controller, we apply logging middleware to the entire controller and routing conditions to specific handlers. You can also apply middleware to specific handlers and routing conditions to the entire controller.

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
builder.Services.AddStellaPolling();
```

For webhook:

```powershell
dotnet add package Stella.AspNetCore
```


```csharp
builder.Services.AddStella();
```

```csharp
app.UseStellaWebhookHandler("/webhook");
```

By default, Stella does not register your webhook with the Telegram API. You need to do this yourself, for example in a Hosted Service.

### Middlewares

The middleware mechanism works similarly to ASP.NET Core:

```csharp
public class MyMiddleware : IMiddleware  
{  
    public async Task ProcessAsync(Update update, Func<Update, Task> next, CancellationToken cancellationToken = default)  
    {  
        // code before handler call
        await next(update);  
        // code after handler call 
    }  
}
```

Middlewares can be **Typed** (created through Dependency Injection) or **Attribute** (created at the stage of initializing a global attribute).

##### Typed Middlewares

```csharp
public class MyTypedMiddleware(IMyDependency dep) : IMiddleware  
{  
    public async Task ProcessAsync(Update update, Func<Update, Task> next, CancellationToken cancellationToken = default)  
    {  
        // code before handler call
        await next(update);  
        // code after handler call 
    }  
}
```

```csharp
[BotController]
[Middleware<MyTypedMiddleware>]
class MyController
```

`MyTypedMiddleware` will be created as a scoped dependency at handler execution time.

##### Attribute Middlewares

```csharp
public class MyMiddlewareAttribute : MiddlewareAttribute<MyMiddlewareAttribute>, IMiddleware  
{  
    public async Task ProcessAsync(Update update, Func<Update, Task> next, CancellationToken cancellationToken = default)  
    {  
        // code before handler call
        await next(update);  
        // code after handler call 
    }  
}
```

```csharp
[BotController]
[MyMiddleware]
class MyController
```

This type of middleware will not be created through DI.

##### Global Middlewares

You can declare global middlewares for the entire application.

For polling:

```csharp
builder.Services.AddStellaPolling(builderAction: bld =>  
{  
    bld.AddGlobalTypedMiddleware(new MiddlewareAttribute<MyTypedMiddleware>());  
    bld.AddGlobalMiddleware(new MyMiddlewareAttribute());
});
```

For webhook:

```csharp
builder.Services.AddStella(builderAction: bld =>  
{  
    bld.AddGlobalTypedMiddleware(new MiddlewareAttribute<MyTypedMiddleware>());  
    bld.AddGlobalMiddleware(new MyMiddlewareAttribute());
});
```

### Conditions

Conditions are rules that determine whether an update should be processed by a specific bot handler/controller.

Example – a built-in attribute condition for checking the update type:

```csharp
public class MessageTypeConditionAttribute(UpdateType updateType)  
    : ConditionAttribute<MessageTypeConditionAttribute>, ICondition  
{  
    public virtual bool Check(Update update)  
    {  
        return update.Type == updateType;  
    }  
}
```

Conditions can also be **Typed** or **Attribute-based**, and you can define global conditions.

##### Typed Condition

```csharp
public class MyTypedCondition(IMyDependency dep) : ICondition  
{  
    public virtual bool Check(Update update)  
    {  
        return dep.DoSmth(update);
    } 
}
```

```csharp
[BotController]
[Condition<MyTypedCondition>]
class MyController
```

##### Attribute Condition

```csharp
public class MessageTypeConditionAttribute(UpdateType updateType)  
    : ConditionAttribute<MessageTypeConditionAttribute>, ICondition  
{  
    public virtual bool Check(Update update)  
    {  
        return update.Type == updateType;  
    }  
}
```

```csharp
[BotController]
[MessageTypeCondition(UpdateType.Message)]
class MyController
```

##### Global Conditions

For polling:

```csharp
builder.Services.AddStellaPolling(builderAction: bld =>  
{  
    bld.AddGlobalTypedCondition(new ConditionAttribute<MyTypedCondition>());  
    bld.AddGlobalCondition(MessageTypeConditionAttribute(UpdateType.Message));
});
```

For webhook:

```csharp
builder.Services.AddStella(builderAction: bld =>  
{  
    bld.AddGlobalTypedCondition(new ConditionAttribute<MyTypedCondition>());  
    bld.AddGlobalCondition(MessageTypeConditionAttribute(UpdateType.Message));
});
```