using SafeNodes.Design;

namespace SafeNodes.Runtime.Execution;

internal interface INodeContextExecutionPipeline<in TNode> 
    where TNode : INode
{
    Task Execute(TNode node, CancellationToken cancellationToken);
}