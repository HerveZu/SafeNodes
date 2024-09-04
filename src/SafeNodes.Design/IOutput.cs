using JetBrains.Annotations;

namespace SafeNodes.Design;

[PublicAPI]
public interface IOutput<in T>
    where T : IValue
{
    void Set(T value);
}