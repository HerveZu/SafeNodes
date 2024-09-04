using SafeNodes.Design;

namespace SafeNodes.Runtime.Execution;

internal class BlueprintRuntime(INodeExecutor nodeExecutor) : IBlueprintRuntime
{
    private readonly Environment _environment = Environment.Empty();
    
    public async Task Execute(Blueprint blueprint, IEventData eventData, CancellationToken cancellationToken = default)
    {
        var runtimeNodes = new RuntimeNodes([..blueprint.Nodes]);
        
        var @event = nodeExecutor.InitializeEvent(blueprint, eventData);

        if (!@event.IsActivated())
        {
            return;
        }
        
        nodeExecutor.OnNodeTriggered(
            async (triggeredNode, ct) =>
            {
                var targetNodes = runtimeNodes
                    .GetTargets(triggeredNode.NodeId, triggeredNode.TriggerReference);

                await ExecuteNodesInParallel(targetNodes, ct);
            });

        nodeExecutor.OnNodeOutputEmitted(
            output => _environment.Add(output.NodeId, output.Reference, output.Value));

        var startNodes = runtimeNodes.GetStart();

        await ExecuteNodesInParallel(startNodes, cancellationToken);
    }

    private async Task ExecuteNodesInParallel(IEnumerable<BlueprintNode> nodes, CancellationToken cancellationToken)
    {
        var environmentSnapshot = _environment.Copy();

        var tasks = nodes
            .Select(node => nodeExecutor.ExecuteNode(node, environmentSnapshot, cancellationToken));

        await Task.WhenAll(tasks);
    }
}