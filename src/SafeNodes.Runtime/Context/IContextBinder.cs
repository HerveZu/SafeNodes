using SafeNodes.Design;

namespace SafeNodes.Runtime.Context;

internal interface IContextBinder
{
    void Bind(string reference, IBindContext requester);
}