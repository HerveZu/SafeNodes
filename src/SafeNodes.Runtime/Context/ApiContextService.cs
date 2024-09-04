namespace SafeNodes.Runtime.Context;

internal sealed record ApiContext : IBoundContext
{
    public required string Reference { get; init; }
}

internal sealed class ApiContextService(IContextBindingProvider contextBindingProvider)
    : IBoundContextResolver<ApiContext>
{
    public ApiContext Resolve(IBindContext requester)
    {
        var value = ResolveOrDefault(requester);

        if (value is not null)
        {
            return value;
        }

        var requesterName = requester.GetType().FullName;

        throw new InvalidOperationException(
            $"Unable to create api context, as requester" +
            $" '{requesterName}' has not been bound with an api reference"
        );
    }

    public ApiContext? ResolveOrDefault(IBindContext requester)
    {
        var reference = contextBindingProvider.GetBoundReference(requester);

        if (reference is null)
        {
            return null;
        }

        return new ApiContext
        {
            Reference = reference
        };
    }
}