using JetBrains.Annotations;
using SafeNodes.Design;
using SafeNodes.Internal.Reflection;
using SafeNodes.Runtime.Inspection;

namespace SafeNodes.Runtime.Schemes;

[PublicAPI]
public sealed record InitializerScheme : IScheme
{
    public required string Reference { get; init; }
    public required string? Docs { get; init; }
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

        if (valueType.IsGenericParameter)
        {
            var generics = valueType.GetGenericParameterConstraints();
            
            if (generics.Length is not 1)
            {
                throw new InvalidOperationException(
                    $"{access.Object} initializer value type is generic. In order to extract scheme, exactly one type constraint must be specified, found {generics.Length}.");
            }

            valueType = generics.Single();
        }

        var valueAccess = apiTypeProvider.GetFromType(valueType);

        return new InitializerScheme
        {
            Reference = access.Api.Reference,
            Docs = access.Api.Docs,
            TypeReference = valueAccess.Api.Reference
        };
    }
}