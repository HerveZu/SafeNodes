using JetBrains.Annotations;

namespace SafeNodes.Runtime;

[PublicAPI]
public sealed record Blueprint
{
    public required BlueprintEvent Event { get; init; }
    public required IEnumerable<BlueprintNode> Nodes { get; init; }
}

[PublicAPI]
public sealed record BlueprintEvent
{
    public required string EventReference { get; init; }
    public IEnumerable<BlueprintEventPropertyActivation> PropertyActivations { get; init; } = [];
}

[PublicAPI]
public sealed record BlueprintEventPropertyActivation
{
    public required string Reference { get; init; }
    public required string InitializerReference { get; init; }
    public required string Value { get; init; }
}

[PublicAPI]
public sealed record BlueprintNode
{
    public required string Id { get; init; }
    public required string NodeReference { get; init; }
    public bool IsEntrypoint { get; init; }
    public BlueprintNodeTrigger? Trigger { get; init; }
    public IEnumerable<BlueprintNodeInput> Inputs { get; init; } = [];
}

[PublicAPI]
public sealed record BlueprintNodeInput
{
    public required string InputReference { get; init; }
    public BlueprintNodeInputSource? Source { get; init; }

    public BlueprintNodeInputInitializer? Initializer { get; init; }
}

[PublicAPI]
public sealed record BlueprintNodeInputInitializer
{
    public required string InitializerReference { get; init; }
    public required string RawValue { get; init; }
}

[PublicAPI]
public sealed record BlueprintNodeInputSource
{
    public required string? NodeId { get; init; }
    public required string OutputReference { get; init; }
    public IEnumerable<string> Properties { get; init; } = [];
}

[PublicAPI]
public sealed record BlueprintNodeTrigger
{
    public required string NodeId { get; init; }
    public required string TriggerReference { get; init; }
}