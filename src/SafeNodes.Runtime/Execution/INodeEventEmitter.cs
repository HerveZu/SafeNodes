namespace SafeNodes.Runtime.Execution;

internal interface INodeEventEmitter
{
    void EmitOutput(EmittedOutput output);
    Task EmitTrigger(TriggeredNode triggeredNode, CancellationToken cancellationToken);
}