using ErrorOr;
using JetBrains.Annotations;

namespace SafeNodes.Design;

[PublicAPI]
public interface INode
{
    Task<ErrorOr<Success>> Execute(CancellationToken cancellationToken);
}