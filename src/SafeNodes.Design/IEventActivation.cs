using JetBrains.Annotations;

namespace SafeNodes.Design;

[PublicAPI]
public interface IEventActivation<out TValue>
    where TValue : IValue
{
    TValue GetActivationValue();
}