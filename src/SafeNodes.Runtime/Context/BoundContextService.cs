namespace SafeNodes.Runtime.Context;

internal sealed class BoundContextService<TContext>(IContextBindingProvider contextBindingProvider)
    : IBoundContextResolver<TContext>, IBoundContextRegister<TContext>
    where TContext : IBoundContext
{
    private readonly Dictionary<string, TContext> _contexts = new();

    public void Register(string reference, TContext context)
    {
        _contexts[reference] = context;
    }

    public TContext Resolve(IBindContext requester)
    {
        var value = ResolveOrDefault(requester);

        if (value is not null)
        {
            return value;
        }

        var requesterName = requester.GetType().FullName;

        throw new InvalidOperationException(
            $"No bound context of type '{typeof(TContext).FullName}'" +
            $" has been found for requester '{requesterName}'"
        );
    }

    public TContext? ResolveOrDefault(IBindContext requester)
    {
        var reference = contextBindingProvider.GetBoundReference(requester);

        return reference is not null
            ? _contexts.GetValueOrDefault(reference)
            : default;
    }
}