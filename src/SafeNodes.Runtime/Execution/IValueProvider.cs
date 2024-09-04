using SafeNodes.Design;

namespace SafeNodes.Runtime.Execution;

internal interface IValueProvider
{
    IValue? GetEventProperty(string propertyReference);
    IValue? GetEventProperty(string propertyReference, IEnumerable<string> properties);
    IValue? GetOutput(string nodeId, string outputReference, IEnumerable<string> properties);
}