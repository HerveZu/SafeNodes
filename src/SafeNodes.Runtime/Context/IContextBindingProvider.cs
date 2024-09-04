using SafeNodes.Design;

namespace SafeNodes.Runtime.Context;

internal interface IContextBindingProvider
{
    string? GetBoundReference(IBindContext requester);
}