using SafeNodes.Design;

namespace SafeNodes.Runtime;

public interface IRuntime
{
    IValueInitializer<TValue> GetInitializer<TValue>(string reference) 
        where TValue : IValue;
    
    IValue? GetOutput(string nodeId, string outputReference, IEnumerable<string> properties);
}