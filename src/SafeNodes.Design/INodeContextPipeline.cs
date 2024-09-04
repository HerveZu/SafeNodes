using ErrorOr;
using JetBrains.Annotations;

namespace SafeNodes.Design;

[PublicAPI]
public delegate Task<IErrorOr> NodeContextPipelineNext();

[PublicAPI]
public interface INodeContextPipeline
{
    Task<IErrorOr> Next(INode node, NodeContextPipelineNext next, CancellationToken cancellationToken);
}