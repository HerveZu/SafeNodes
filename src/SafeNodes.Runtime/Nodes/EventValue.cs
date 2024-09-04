using SafeNodes.Design;
using SafeNodes.Runtime.Context;
using SafeNodes.Runtime.Execution;

namespace SafeNodes.Runtime.Nodes;

internal sealed class EventValue<TValue>(
    IValueProvider valueProvider,
    IContextResolver contextResolver
)
    : IBindContext, IEventValue<TValue>, IEventValueDefinition<TValue>
    where TValue : IValue
{
    private TValue? _value;

    public TValue GetValue()
    {
        if (_value is not null)
        {
            return _value;
        }

        var apiContext = contextResolver.Bound<ApiContext>(this);
        var value = valueProvider.GetEventProperty(apiContext.Reference);

        if (value is null)
        {
            throw new InvalidOperationException("Event value is null");
        }

        return (TValue)value;
    }

    public void DefineValue(TValue value)
    {
        if (_value is not null)
        {
            throw new InvalidOperationException("Event value is already defined");
        }

        _value = value;
    }

    public TValue GetDefined()
    {
        return _value ?? throw new InvalidOperationException("Event value is not defined");
    }
}