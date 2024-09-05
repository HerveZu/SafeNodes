using ErrorOr;
using JetBrains.Annotations;
using SafeNodes.Design;

namespace SafeNodes.Runtime;

/// <summary>
/// The next pipe in the pipeline.
/// </summary>
[PublicAPI]
public delegate Task<IErrorOr> NodeContextPipelineNext();

/// <summary>
/// Defines a pipe around the execution of every node assignable to <see cref="TNode"/>.
/// </summary>
/// <typeparam name="TNode">The type of the node to match</typeparam>
[PublicAPI]
public interface INodeContextPipeline<in TNode> 
    where TNode : INode
{
    /// <summary>
    /// Invoke this pipe as the next action from the previous pipe.
    /// </summary>
    /// <param name="node">The node form the context</param>
    /// <param name="next">The next action in the pipeline. If this is the last pipe, the node will be executed</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>The pipe execution error if any</returns>
    Task<IErrorOr> Next(TNode node, NodeContextPipelineNext next, CancellationToken cancellationToken);
}

/// <summary>
/// Defines a pipe around the execution of every <see cref="INode"/>.
/// </summary>
[PublicAPI]
public interface INodeContextPipeline : INodeContextPipeline<INode>;