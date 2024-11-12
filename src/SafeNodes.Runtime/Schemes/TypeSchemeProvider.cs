using JetBrains.Annotations;
using SafeNodes.Design;
using SafeNodes.Runtime.Inspection;

namespace SafeNodes.Runtime.Schemes;

[PublicAPI]
public sealed record TypeScheme : IScheme
{
    public required string Reference { get; init; }
    public required string? Docs { get; init; }
    public required IEnumerable<string> Is { get; init; }
    public required IEnumerable<TypePropertyScheme> Properties { get; init; }
}

[PublicAPI]
public sealed record TypePropertyScheme
{
    public required string Reference { get; init; }
    public required string? Docs { get; init; }
    public required string TypeReference { get; init; }
}

internal sealed class TypeSchemeProvider(
    IApiTypeProvider apiTypeProvider,
    IPropertyAccessor propertyAccessor
)
    : ISchemeProvider<TypeScheme>
{
    public IEnumerable<TypeScheme> GetSchemes()
    {
        return apiTypeProvider
            .GetAssignableTo(typeof(IValue))
            .Select(ToScheme);
    }

    private TypeScheme ToScheme(ApiObjectAccess<Type> access)
    {
        var parents = new List<Type>();

        if (access.Object.BaseType is not null)
        {
            parents.Add(access.Object.BaseType);
        }
        
        parents.AddRange(access.Object.GetInterfaces());

        var parentsReferences = parents
            .Select(parent => apiTypeProvider.GetFromTypeOrDefault(parent)?.Api.Reference)
            .OfType<string>();
        
        var properties = propertyAccessor
            .GetPropertiesType(access.Object, typeof(IValue))
            .Select(ToPropertyScheme);

        return new TypeScheme
        {
            Reference = access.Api.Reference,
            Docs = access.Api.Docs,
            Is = parentsReferences,
            Properties = properties
        };
    }

    private TypePropertyScheme ToPropertyScheme(ApiObjectAccess<Type> access)
    {
        var typeAccess = apiTypeProvider.GetFromType(access.Object);

        return new TypePropertyScheme
        {
            Reference = access.Api.Reference,
            Docs = access.Api.Docs,
            TypeReference = typeAccess.Api.Reference
        };
    }
}