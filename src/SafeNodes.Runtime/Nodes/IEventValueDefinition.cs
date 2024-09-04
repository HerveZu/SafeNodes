using SafeNodes.Design;

namespace SafeNodes.Runtime.Nodes;

internal interface IEventValueDefinition<out TValue>
    where TValue : IValue
{
    TValue GetDefined();
}