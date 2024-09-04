using SafeNodes.Design;
using SafeNodes.Runtime.Context;
using SafeNodes.Runtime.Execution;
using SafeNodes.Runtime.Inspection;

namespace SafeNodes.Runtime.Nodes;

internal sealed record SourceContext : IBoundContext
{
    public required string? NodeId { get; init; }
    public required string OutputReference { get; init; }
    public required IEnumerable<string> Properties { get; init; }
}

internal sealed record InitializerContext : IBoundContext
{
    public required string Reference { get; init; }
    public required string Value { get; init; }
}

internal abstract class Input<T>(
    IApiServiceProvider apiServiceProvider,
    IContextResolver contextResolver,
    IValueProvider valueProvider
)
    : IBindContext
    where T : IValue
{
    protected T? GetValue()
    {
        var apiContext = contextResolver.Bound<ApiContext>(this);
        var value = GetSourceValue() ?? GetDefaultValue();

        if (value is null)
        {
            return default;
        }

        if (value is not T parsed)
        {
            throw new InvalidOperationException(
                $"Value type '{value.GetType().FullName}'" +
                $" is not assignable to input with reference '{apiContext.Reference}' value type '{typeof(T).FullName}'"
            );
        }

        return parsed;
    }

    private IValue? GetSourceValue()
    {
        var sourceContext = contextResolver.BoundOrDefault<SourceContext>(this);

        if (sourceContext is null)
        {
            return default;
        }

        return sourceContext.NodeId is null
            ? valueProvider.GetEventProperty(sourceContext.OutputReference, sourceContext.Properties)
            : valueProvider.GetOutput(sourceContext.NodeId, sourceContext.OutputReference, sourceContext.Properties);
    }

    private IValue? GetDefaultValue()
    {
        var initializerContext = contextResolver.BoundOrDefault<InitializerContext>(this);

        if (initializerContext is null)
        {
            return default;
        }

        var initializerAccess = apiServiceProvider
            .GetByReference<IValueInitializer<T>>(initializerContext.Reference);

        return initializerAccess.Object.InitializeValue(initializerContext.Value);
    }
}