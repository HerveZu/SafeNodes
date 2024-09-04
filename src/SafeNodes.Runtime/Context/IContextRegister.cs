using SafeNodes.Design;

namespace SafeNodes.Runtime.Context;

internal interface IContextRegister
{
    void RegisterScoped<TContext>(TContext context)
        where TContext : IScopedContext;

    void RegisterBound<TContext>(string reference, TContext context)
        where TContext : IBoundContext;
}