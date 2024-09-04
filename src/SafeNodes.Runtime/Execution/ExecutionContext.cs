using SafeNodes.Design;
using SafeNodes.Runtime.Context;

namespace SafeNodes.Runtime.Execution;

internal sealed record GlobalExecutionContext : IScopedContext
{
    public required IEventBone Event { get; init; }
}

internal sealed record NodeExecutionContext : IScopedContext
{
    public required string NodeId { get; init; }
    public required IReadOnlyEnvironment Environment { get; init; }
}