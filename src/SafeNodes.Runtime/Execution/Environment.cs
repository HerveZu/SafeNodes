using JetBrains.Annotations;
using SafeNodes.Design;

namespace SafeNodes.Runtime.Execution;

internal sealed record EnvironmentKey(
    [UsedImplicitly] string NodeId,
    [UsedImplicitly] string OutputReference
);

internal interface IReadOnlyEnvironment
{
    IValue? Read(string nodeId, string reference);
}

internal sealed class Environment(Dictionary<EnvironmentKey, IValue> values) : IReadOnlyEnvironment
{
    public IValue? Read(string nodeId, string reference)
    {
        var key = new EnvironmentKey(nodeId, reference);

        return values.GetValueOrDefault(key);
    }

    public static Environment Empty()
    {
        return new Environment(new Dictionary<EnvironmentKey, IValue>());
    }

    public Environment Copy()
    {
        return new Environment(values.ToDictionary());
    }

    public void Add(string nodeId, string reference, IValue value)
    {
        var key = new EnvironmentKey(nodeId, reference);
        values[key] = value;
    }
}