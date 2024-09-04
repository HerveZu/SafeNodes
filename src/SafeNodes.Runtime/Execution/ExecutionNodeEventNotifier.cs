namespace SafeNodes.Runtime.Execution;

internal sealed class ExecutionNodeEventNotifier : INodeEventNotifier, INodeEventEmitter
{
    private readonly List<NodeTriggeredCallback> _nodeTriggeredCallbacks = [];
    private readonly List<OutputEmittedCallback> _outputEmittedCallbacks = [];

    public void EmitOutput(EmittedOutput output)
    {
        _outputEmittedCallbacks.ForEach(callback => callback(output));
    }

    public async Task EmitTrigger(TriggeredNode triggeredNode, CancellationToken cancellationToken)
    {
        var tasks = _nodeTriggeredCallbacks
            .Select(callback => callback(triggeredNode, cancellationToken));

        await Task.WhenAll(tasks);
    }

    public void OnOutputEmitted(OutputEmittedCallback callback)
    {
        _outputEmittedCallbacks.Add(callback);
    }

    public void OnTriggered(NodeTriggeredCallback callback)
    {
        _nodeTriggeredCallbacks.Add(callback);
    }
}