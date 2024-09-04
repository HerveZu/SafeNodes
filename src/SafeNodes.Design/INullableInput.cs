using JetBrains.Annotations;

namespace SafeNodes.Design;

[PublicAPI]
public interface INullableInput<T> : IAnyInput
    where T : IValue
{
    T? GetOrDefault();
    T GetOrDefault(T defaultValue);
}