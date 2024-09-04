using SafeNodes.Design;

namespace SafeNodes.Runtime.Context;

internal sealed class ContextBindingService : IContextBinder, IContextBindingProvider
{
    private readonly Dictionary<object, string> _references = new();

    public void Bind(string reference, IBindContext requester)
    {
        _references[requester] = reference;
    }

    public string? GetBoundReference(IBindContext requester)
    {
        var hasReference = _references.TryGetValue(requester, out var reference);

        return hasReference ? reference : null;
    }
}