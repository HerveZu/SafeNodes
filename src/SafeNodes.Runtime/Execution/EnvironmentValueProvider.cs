using SafeNodes.Design;
using SafeNodes.Runtime.Context;
using SafeNodes.Runtime.Inspection;
using SafeNodes.Runtime.Nodes;

namespace SafeNodes.Runtime.Execution;

internal sealed class EnvironmentValueProvider(
    IContextResolver contextResolver,
    IPropertyAccessor propertyAccessor
) : IValueProvider
{
    public IValue? GetEventProperty(string propertyReference)
    {
        var globalExecutionContext = contextResolver.Scoped<GlobalExecutionContext>();

        var eventProperties = propertyAccessor
            .GetValues<IEventValueDefinition<IValue>>(globalExecutionContext.Event)
            .ToArray();

        var matchingProperty = eventProperties
            .FirstOrDefault(propertyAccess => propertyAccess.Reference == propertyReference);

        return matchingProperty?.Object.GetDefined();
    }

    public IValue? GetEventProperty(string propertyReference, IEnumerable<string> properties)
    {
        var eventValue = GetEventProperty(propertyReference);

        return GetPropertyOrDefault(eventValue, properties);
    }

    public IValue? GetOutput(string nodeId, string outputReference, IEnumerable<string> properties)
    {
        var nodeExecutionContext = contextResolver.Scoped<NodeExecutionContext>();
        var outputValue = nodeExecutionContext.Environment.Read(nodeId, outputReference);

        return GetPropertyOrDefault(outputValue, properties);
    }

    private IValue? GetPropertyOrDefault(IValue? value, IEnumerable<string> properties)
    {
        if (value is null)
        {
            return default;
        }

        foreach (var property in properties)
        {
            var valuePropertyAccess = propertyAccessor
                .GetValues<IValue>(value)
                .First(propertyAccess => propertyAccess.Reference == property);

            value = valuePropertyAccess.Object;
        }

        return value;
    }
}