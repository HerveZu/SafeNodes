namespace SafeNodes.Runtime.Context;

internal interface IScopedContextResolver<out TContext>
    where TContext : IScopedContext
{
    TContext Resolve();
}