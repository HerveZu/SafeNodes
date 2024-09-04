using SafeNodes.Design;

namespace SafeNodes.Runtime.Execution;

internal interface INodeContextExecutionPipeline
{
    Task Execute(INode node, CancellationToken cancellationToken);
}