using ErrorOr;
using SafeNodes.Design;

namespace SafeNodes.Runtime.Execution;

internal sealed class NodeContextStackExecutionPipeline<TNode>(INodeContextPipeline<TNode>[] pipelines) 
    : INodeContextExecutionPipeline<TNode> where TNode : INode
{
    public async Task Execute(TNode node, CancellationToken cancellationToken)
    {
        var pipelinesStack = new Stack<INodeContextPipeline<TNode>>(pipelines.Reverse());

        await Next();
        return;

        async Task<IErrorOr> Next()
        {
            if (pipelinesStack.TryPop(out var nextPipeline))
            {
                return await nextPipeline.Next(node, Next, cancellationToken);
            }

            return await node.Execute(cancellationToken);
        }
    }
}