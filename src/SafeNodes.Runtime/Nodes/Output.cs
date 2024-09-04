using SafeNodes.Design;
using SafeNodes.Runtime.Context;
using SafeNodes.Runtime.Execution;

namespace SafeNodes.Runtime.Nodes;

internal sealed class Output<T>(
    INodeEventEmitter eventEmitter,
    IContextResolver contextResolver
)
    : IOutput<T>, IBindContext
    where T : IValue
{
    public void Set(T value)
    {
        var apiContext = contextResolver.Bound<ApiContext>(this);
        var executionContext = contextResolver.Scoped<NodeExecutionContext>();

        eventEmitter.EmitOutput(
            new EmittedOutput
            {
                NodeId = executionContext.NodeId,
                Reference = apiContext.Reference,
                Value = value
            });
    }
}