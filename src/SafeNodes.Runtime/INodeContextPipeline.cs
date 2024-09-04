using ErrorOr;
using JetBrains.Annotations;
using SafeNodes.Design;

namespace SafeNodes.Runtime;

[PublicAPI]
public delegate Task<IErrorOr> NodeContextPipelineNext();

[PublicAPI]
public interface INodeContextPipeline
{
    Task<IErrorOr> Next(INode node, NodeContextPipelineNext next, CancellationToken cancellationToken);
}