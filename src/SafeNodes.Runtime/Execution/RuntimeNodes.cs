using System.Collections.Immutable;

namespace SafeNodes.Runtime.Execution;

internal sealed class RuntimeNodes(ImmutableArray<BlueprintNode> nodes)
{
    public BlueprintNode GetById(string nodeId)
    {
        return nodes.Single(node => node.Id == nodeId);
    }

    public IEnumerable<BlueprintNode> GetStart()
    {
        return nodes.Where(node => node.IsEntrypoint);
    }

    public IEnumerable<BlueprintNode> GetTargets(
        string triggeringNodeId,
        string triggerReference)
    {
        return nodes
            .Where(node => node.Trigger is not null)
            .Where(node => node.Trigger!.NodeId == triggeringNodeId)
            .Where(node => node.Trigger!.TriggerReference == triggerReference);
    }

    public IEnumerable<BlueprintNode> GetConnected()
    {
        return nodes.Where(node => GetRootNode(node)?.IsEntrypoint ?? false);
    }

    private BlueprintNode? GetRootNode(BlueprintNode node)
    {
        if (node.IsEntrypoint)
        {
            return node;
        }

        if (node.Trigger is null)
        {
            return null;
        }

        var parentNode = GetById(node.Trigger.NodeId);

        // ReSharper disable once TailRecursiveCall
        return GetRootNode(parentNode);
    }
}