namespace Stella.Middlewares;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class MiddlewareAttribute<TMiddleware> : Attribute, ITypedMiddleware
    where TMiddleware : IMiddleware
{
    public Type Middleware => typeof(TMiddleware);
}