using JetBrains.Annotations;

namespace SafeNodes.Design;

[PublicAPI]
public interface ITrigger
{
    Task Trigger(CancellationToken cancellationToken);
}