using SafeNodes.Design;
using SafeNodes.Runtime.Context;
using SafeNodes.Runtime.Execution;

namespace SafeNodes.Runtime.Nodes;

internal sealed class ExecutionTrigger(INodeEventEmitter nodeEventEmitter, IContextResolver contextResolver)
    : ITrigger, IBindContext
{
    public async Task Trigger(CancellationToken cancellationToken)
    {
        var apiContext = contextResolver.Bound<ApiContext>(this);
        var nodeExecutionContext = contextResolver.Scoped<NodeExecutionContext>();

        await nodeEventEmitter.EmitTrigger(
            new TriggeredNode
            {
                TriggerReference = apiContext.Reference,
                NodeId = nodeExecutionContext.NodeId
            },
            cancellationToken);
    }
}