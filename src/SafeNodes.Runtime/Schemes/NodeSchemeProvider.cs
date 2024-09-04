using JetBrains.Annotations;
using SafeNodes.Design;
using SafeNodes.Runtime.Inspection;

namespace SafeNodes.Runtime.Schemes;

[PublicAPI]
public sealed record NodeScheme : IScheme
{
    public required string Reference { get; init; }
    public required IEnumerable<NodeInputScheme> Inputs { get; init; }
    public required IEnumerable<NodeOutputScheme> Outputs { get; init; }
    public required IEnumerable<NodeTriggerScheme> Triggers { get; init; }
}

[PublicAPI]
public sealed record NodeInputScheme
{
    public required string Reference { get; init; }
    public required string TypeReference { get; init; }
}

[PublicAPI]
public sealed record NodeOutputScheme
{
    public required string Reference { get; init; }
    public required string TypeReference { get; init; }
}

[PublicAPI]
public sealed record NodeTriggerScheme
{
    public required string Reference { get; init; }
}


internal sealed class NodeSchemeProvider(
    IApiTypeProvider apiTypeProvider,
    IPropertyAccessor propertyAccessor
)
    : ISchemeProvider<NodeScheme>
{
    public IEnumerable<NodeScheme> GetSchemes()
    {
        return apiTypeProvider
            .GetAssignableTo(typeof(INode))
            .Select(ToNodeScheme);
    }

    private NodeScheme ToNodeScheme(ApiObjectAccess<Type> access)
    {
        var inputs = propertyAccessor
            .GetPropertiesType(access.Object, typeof(IAnyInput))
            .Select(ToInputScheme);

        var outputs = propertyAccessor
            .GetPropertiesType(access.Object, typeof(IOutput<>))
            .Select(ToOutputScheme);

        var triggers = propertyAccessor
            .GetPropertiesType(access.Object, typeof(ITrigger))
            .Select(ToTriggerScheme);
        
        return new NodeScheme
        {
            Reference = access.Reference,
            Inputs = inputs,
            Outputs = outputs,
            Triggers = triggers
        };
    }

    private NodeInputScheme ToInputScheme(ApiObjectAccess<Type> access)
    {
        var valueType = access.Object
            .GetGenericArguments()
            .First();

        var valueAccess = apiTypeProvider.GetFromType(valueType);

        return new NodeInputScheme
        {
            Reference = access.Reference,
            TypeReference = valueAccess.Reference
        };
    }

    private NodeOutputScheme ToOutputScheme(ApiObjectAccess<Type> access)
    {
        var valueType = access.Object
            .GetGenericArguments()
            .First();

        var valueAccess = apiTypeProvider.GetFromType(valueType);

        return new NodeOutputScheme
        {
            Reference = access.Reference,
            TypeReference = valueAccess.Reference
        };
    }

    private static NodeTriggerScheme ToTriggerScheme(ApiObjectAccess<Type> access)
    {
        return new NodeTriggerScheme
        {
            Reference = access.Reference
        };
    }
}