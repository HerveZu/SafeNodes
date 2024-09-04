using SafeNodes.Design;

namespace SafeNodes.Runtime.Execution;

internal sealed record TriggeredNode
{
    public required string NodeId { get; init; }
    public required string TriggerReference { get; init; }
}

internal sealed record EmittedOutput
{
    public required string NodeId { get; init; }
    public required string Reference { get; init; }
    public required IValue Value { get; init; }
}

internal delegate Task NodeTriggeredCallback(TriggeredNode node, CancellationToken cancellationToken);

internal delegate void OutputEmittedCallback(EmittedOutput output);

internal interface INodeExecutor
{
    IEventBone InitializeEvent(Blueprint blueprint, IEventData eventData);
    Task ExecuteNode(BlueprintNode node, IReadOnlyEnvironment environment, CancellationToken cancellationToken);
    void OnNodeOutputEmitted(OutputEmittedCallback callback);
    void OnNodeTriggered(NodeTriggeredCallback callback);
}