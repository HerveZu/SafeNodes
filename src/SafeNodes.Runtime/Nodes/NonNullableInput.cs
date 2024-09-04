using SafeNodes.Design;
using SafeNodes.Runtime.Context;
using SafeNodes.Runtime.Execution;
using SafeNodes.Runtime.Inspection;

namespace SafeNodes.Runtime.Nodes;

internal sealed class NonNullableInput<T>(
    IApiServiceProvider apiServiceProvider,
    IContextResolver contextResolver,
    IValueProvider valueProvider
)
    : Input<T>(apiServiceProvider, contextResolver, valueProvider), IInput<T>
    where T : IValue
{
    public T Get()
    {
        var value = GetValue();

        if (value is not null)
        {
            return value;
        }

        throw new InvalidOperationException("Non nullable input value is null");
    }
}