namespace SafeNodes.Runtime.Context;

internal interface IScopedContextRegister<in TContext>
    where TContext : IScopedContext
{
    void Register(TContext context);
}