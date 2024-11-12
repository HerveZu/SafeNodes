using SafeNodes.Design;
using SafeNodes.Runtime.Execution;
using SafeNodes.Runtime.Inspection;

namespace SafeNodes.Runtime;

internal sealed class InternalRuntimeProxy(IValueProvider valueProvider, IApiServiceProvider apiServiceProvider)
    : IRuntime
{
    public IValueInitializer<TValue> GetInitializer<TValue>(string reference) where TValue : IValue
    {
        return apiServiceProvider.GetByReference<IValueInitializer<TValue>>(reference).Object;
    }

    public IValue? GetOutput(string nodeId, string outputReference, IEnumerable<string> properties)
    {
        return valueProvider.GetOutput(nodeId, outputReference, properties);
    }
}