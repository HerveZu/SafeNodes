using JetBrains.Annotations;

namespace SafeNodes.Design;

[PublicAPI]
public interface IInput<out T> : IAnyInput
    where T : IValue
{
    T Get();
}