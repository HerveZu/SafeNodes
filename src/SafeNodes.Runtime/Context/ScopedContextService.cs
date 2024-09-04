namespace SafeNodes.Runtime.Context;

internal sealed class ScopedContextService<TContext>
    : IScopedContextRegister<TContext>, IScopedContextResolver<TContext>
    where TContext : IScopedContext
{
    private TContext? _context;

    public void Register(TContext context)
    {
        _context = context;
    }

    public TContext Resolve()
    {
        return _context
               ?? throw new InvalidOperationException(
                   $"No scoped context of type '{typeof(TContext).FullName}' has been found"
               );
    }
}