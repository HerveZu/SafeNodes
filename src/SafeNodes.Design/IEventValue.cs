using JetBrains.Annotations;

namespace SafeNodes.Design;

[PublicAPI]
public interface IEventValue<TValue>
    where TValue : IValue
{
    TValue GetValue();
    void DefineValue(TValue value);
}