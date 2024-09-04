using SafeNodes.Design;
using SafeNodes.Runtime.Context;
using SafeNodes.Runtime.Execution;
using SafeNodes.Runtime.Inspection;

namespace SafeNodes.Runtime.Nodes;

internal sealed record EventActivationContext : IScopedContext
{
    public required IEnumerable<BlueprintEventPropertyActivation> EventActivations { get; init; }
}

internal sealed class EventActivation<TValue>(
    IContextResolver contextResolver,
    IApiServiceProvider apiServiceProvider
)
    : IBindContext, IEventActivation<TValue>
    where TValue : IValue
{
    public TValue GetActivationValue()
    {
        var executionContext = contextResolver.Scoped<EventActivationContext>();
        var apiContext = contextResolver.Bound<ApiContext>(this);

        var activation = executionContext.EventActivations
            .Single(activation => activation.Reference == apiContext.Reference);

        var initializer = apiServiceProvider
            .GetByReference<IValueInitializer<TValue>>(activation.InitializerReference);

        if (initializer is null)
        {
            throw new InvalidOperationException(
                $"Value '{typeof(TValue).FullName}' must define" +
                $" an initializer in order to use it as an event filter"
            );
        }

        return initializer.Object.InitializeValue(activation.Value);
    }
}