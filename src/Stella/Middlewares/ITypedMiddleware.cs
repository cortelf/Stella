namespace Stella.Middlewares;

public interface ITypedMiddleware
{
    Type Middleware { get; }
}