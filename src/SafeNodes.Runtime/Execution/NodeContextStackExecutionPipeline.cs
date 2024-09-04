using ErrorOr;
using SafeNodes.Design;

namespace SafeNodes.Runtime.Execution;

internal sealed class NodeContextStackExecutionPipeline(INodeContextPipeline[] pipelines) 
    : INodeContextExecutionPipeline
{
    public async Task Execute(INode node, CancellationToken cancellationToken)
    {
        var pipelinesStack = new Stack<INodeContextPipeline>(pipelines.Reverse());

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