namespace SafeNodes.Runtime.Context;

internal interface IBoundContextResolver<out TContext>
    where TContext : IBoundContext
{
    TContext Resolve(IBindContext requester);
    TContext? ResolveOrDefault(IBindContext requester);
}