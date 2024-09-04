using SafeNodes.Design;

namespace SafeNodes.Runtime.Execution;

/// <inheritdoc cref="IBlueprintRuntime"/>
internal class BlueprintRuntime(INodeExecutor nodeExecutor) : IBlueprintRuntime
{
    private readonly Environment _environment = Environment.Empty();

    public async Task ExecuteMandatory(
        Blueprint blueprint,
        IEventData eventData,
        CancellationToken cancellationToken = default)
    {
        var @event = nodeExecutor.InitializeEvent(blueprint, eventData);

        if (@event is null)
        {
            throw new MismatchingEventDataException(eventData.GetType(), blueprint.Event.EventReference);
        }

        await ExecuteWithEvent(blueprint, @event, cancellationToken);
    }

    public async Task<bool> Execute(
        Blueprint blueprint,
        IEventData eventData,
        CancellationToken cancellationToken = default)
    {
        var @event = nodeExecutor.InitializeEvent(blueprint, eventData);

        if (@event is null)
        {
            return false;
        }

        await ExecuteWithEvent(blueprint, @event, cancellationToken);
        return true;
    }

    private async Task ExecuteWithEvent(Blueprint blueprint, IEventBone @event, CancellationToken cancellationToken)
    {
        if (!@event.IsActivated())
        {
            return;
        }

        var runtimeNodes = new RuntimeNodes([..blueprint.Nodes]);

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