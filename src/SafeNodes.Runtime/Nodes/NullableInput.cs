using SafeNodes.Design;
using SafeNodes.Runtime.Context;
using SafeNodes.Runtime.Execution;
using SafeNodes.Runtime.Inspection;

namespace SafeNodes.Runtime.Nodes;

internal sealed class NullableInput<T>(
    IApiServiceProvider apiServiceProvider,
    IContextResolver contextResolver,
    IValueProvider valueProvider
)
    : Input<T>(apiServiceProvider, contextResolver, valueProvider), INullableInput<T>
    where T : IValue
{
    public T? GetOrDefault()
    {
        return GetValue();
    }

    public T GetOrDefault(T defaultValue)
    {
        return GetValue() ?? defaultValue;
    }
}