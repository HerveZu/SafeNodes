using JetBrains.Annotations;
using SafeNodes.Design;
using SafeNodes.Internal.Reflexion;
using SafeNodes.Runtime.Inspection;

namespace SafeNodes.Runtime.Schemes;

[PublicAPI]
public sealed record InitializerScheme : IScheme
{
    public required string Reference { get; init; }
    public required string TypeReference { get; init; }
}

internal sealed class InitializerSchemeProvider(IApiTypeProvider apiTypeProvider) : ISchemeProvider<InitializerScheme>
{
    public IEnumerable<InitializerScheme> GetSchemes()
    {
        return apiTypeProvider
            .GetAssignableTo(typeof(IValueInitializer<>))
            .Select(ToScheme);
    }

    private InitializerScheme ToScheme(ApiObjectAccess<Type> access)
    {
        var valueType = access.Object
            .GetGenericsFromInterface(typeof(IValueInitializer<>))
            .First();

        var valueAccess = apiTypeProvider.GetFromType(valueType);

        return new InitializerScheme
        {
            Reference = access.Reference,
            TypeReference = valueAccess.Reference
        };
    }
}