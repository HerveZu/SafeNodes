namespace SafeNodes.Runtime.Inspection;

internal sealed record ApiObjectAccess<T>(
    string Reference,
    T Object
);