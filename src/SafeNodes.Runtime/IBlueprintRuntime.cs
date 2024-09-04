using JetBrains.Annotations;
using SafeNodes.Design;

namespace SafeNodes.Runtime;

[PublicAPI]
public interface IBlueprintRuntime
{
    Task Execute(Blueprint blueprint, IEventData eventData, CancellationToken cancellationToken = default);
}