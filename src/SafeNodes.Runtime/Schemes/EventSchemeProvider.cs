using JetBrains.Annotations;
using SafeNodes.Design;
using SafeNodes.Runtime.Inspection;

namespace SafeNodes.Runtime.Schemes;

[PublicAPI]
public sealed record EventScheme : IScheme
{
    public required string Reference { get; init; }
    public required IEnumerable<EventPropertyScheme> Properties { get; init; }
    public required IEnumerable<EventActivationScheme> Activations { get; init; }
}

[PublicAPI]
public sealed record EventPropertyScheme
{
    public required string Reference { get; init; }
    public required string TypeReference { get; init; }
}

[PublicAPI]
public sealed record EventActivationScheme
{
    public required string Reference { get; init; }
    public required string TypeReference { get; init; }
}

internal sealed class EventSchemeProvider(
    IApiTypeProvider apiTypeProvider,
    IPropertyAccessor propertyAccessor
)
    : ISchemeProvider<EventScheme>
{
    public IEnumerable<EventScheme> GetSchemes()
    {
        return apiTypeProvider
            .GetAssignableTo(typeof(IEvent<>))
            .Select(ToScheme);
    }

    private EventScheme ToScheme(ApiObjectAccess<Type> access)
    {
        var properties = propertyAccessor
            .GetPropertiesType(access.Object, typeof(IEventValue<>))
            .Select(ToPropertyScheme);

        var activations = propertyAccessor
            .GetPropertiesType(access.Object, typeof(IEventActivation<>))
            .Select(ToActivationScheme);

        return new EventScheme
        {
            Reference = access.Reference,
            Properties = properties,
            Activations = activations
        };
    }

    private EventPropertyScheme ToPropertyScheme(ApiObjectAccess<Type> access)
    {
        var valueType = access.Object
            .GetGenericArguments()
            .First();

        var typeAccess = apiTypeProvider.GetFromType(valueType);

        return new EventPropertyScheme
        {
            Reference = access.Reference,
            TypeReference = typeAccess.Reference
        };
    }

    private EventActivationScheme ToActivationScheme(ApiObjectAccess<Type> access)
    {
        var valueType = access.Object
            .GetGenericArguments()
            .First();

        var typeAccess = apiTypeProvider.GetFromType(valueType);

        return new EventActivationScheme
        {
            Reference = access.Reference,
            TypeReference = typeAccess.Reference
        };
    }
}