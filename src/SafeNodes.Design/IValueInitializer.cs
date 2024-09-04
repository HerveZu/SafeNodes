using JetBrains.Annotations;

namespace SafeNodes.Design;

[PublicAPI]
public interface IValue;

[PublicAPI]
public interface IValueInitializer<out TValue>
    where TValue : IValue
{
    TValue InitializeValue(string rawValue);
}