using SafeNodes.Design;

namespace SafeNodes.Runtime.Inspection;

internal sealed record ApiObjectAccess<T>(
    ApiAttribute Api,
    T Object
);