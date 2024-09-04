namespace SafeNodes.Runtime.Context;

internal interface IContextResolver
{
    TContext Scoped<TContext>()
        where TContext : IScopedContext;

    TContext Bound<TContext>(IBindContext requester)
        where TContext : IBoundContext;

    TContext? BoundOrDefault<TContext>(IBindContext requester)
        where TContext : IBoundContext;
}