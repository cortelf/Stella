using Stella.Conditions;
using Stella.Middlewares;
using Telegram.Bot.Types;

namespace Stella.Handlers;

public abstract class UpdateHandler: IUpdateHandler, IConfigurableHandler
{
    private IList<ConditionContainer>? _conditions;
    private IList<ConditionDescriptorContainer>? _conditionDescriptors;
    private IList<MiddlewareContainer>? _middlewares;
    private IList<MiddlewareDescriptorContainer>? _middlewareDescriptors;

    IList<ConditionContainer>? IUpdateHandler.Conditions
    {
        get => _conditions;
        set => _conditions = value;
    }

    private int _priority = 0;
    int IConfigurableHandler.Priority => _priority;
    IList<ConditionDescriptorContainer>? IConfigurableHandler.ConditionDescriptors => _conditionDescriptors;

    IList<MiddlewareContainer>? IUpdateHandler.Middlewares
    {
        get => _middlewares;
        set => _middlewares = value;
    }

    IList<MiddlewareDescriptorContainer>? IConfigurableHandler.MiddlewareDescriptors => _middlewareDescriptors;

    public void SetPriority(int priority)
    {
        _priority = priority;
    }
    public void UseMiddleware(MiddlewareDescriptorContainer container)
    {
        if (_middlewareDescriptors == null)
            _middlewareDescriptors = new List<MiddlewareDescriptorContainer>();
        _middlewareDescriptors.Add(container);
    }

    public void UseMiddleware<TMiddleware>(object? context)
    where TMiddleware: IAsyncMiddleware
    {
        UseMiddleware(new MiddlewareDescriptorContainer(typeof(TMiddleware), context));
    }
    
    public void UseMiddleware<TMiddleware, TContext>(TContext context)
        where TMiddleware: AsyncMiddleware<TContext>
    {
        UseMiddleware(new MiddlewareDescriptorContainer(typeof(TMiddleware), context));
    }
    
    public void UseMiddleware<TMiddleware>()
        where TMiddleware: AsyncMiddleware
    {
        UseMiddleware(new MiddlewareDescriptorContainer(typeof(TMiddleware), null));
    }
    
    public void UseCondition(ConditionDescriptorContainer container)
    {
        if (_conditionDescriptors == null)
            _conditionDescriptors = new List<ConditionDescriptorContainer>();
        _conditionDescriptors.Add(container);
    }
    
    public void UseCondition<TCondition>(object? context)
        where TCondition: IAsyncCondition
    {
        UseCondition(new ConditionDescriptorContainer(typeof(TCondition), context));
    }
    
    public void UseCondition<TCondition, TContext>(TContext context)
        where TCondition: AsyncCondition<TContext>
    {
        UseCondition(new ConditionDescriptorContainer(typeof(TCondition), context));
    }
    
    public void UseCondition<TCondition>()
        where TCondition: AsyncCondition
    {
        UseCondition(new ConditionDescriptorContainer(typeof(TCondition), null));
    }

    public virtual void Configure()
    {
    }
    
    public async ValueTask<bool> CanHandleAsync(Update update, CancellationToken cancellationToken = default)
    {
        foreach (var condition in _conditions!)
        {
            if (!await condition.Condition.CheckAsync(update, condition.Context, cancellationToken))
                return false;
        }
        return true;
    }

    public Task HandleAsync(Update update, CancellationToken cancellationToken = default)
    {
        var updateContext = new UpdateContext(update.Id);
        Func<Update, Task> methodFunc = upd => HandleAsync(upd, updateContext, cancellationToken);
        var funcMiddleware = _middlewares!
            .Reverse()
            .Aggregate(methodFunc, (func, middleware) => upd => middleware.Middleware.ProcessAsync(upd, middleware.Context, func, cancellationToken));
        return funcMiddleware(update);
    }

    public abstract Task HandleAsync(Update update, UpdateContext updateContext, CancellationToken cancellationToken = default);
}