using JetBrains.Annotations;

namespace SafeNodes.Runtime.Inspection;

[PublicAPI]
public sealed class InvalidApiReferenceException(string reference, Type type)
    : InvalidOperationException(
        $"""
         Invalid reference '{reference}' for the type '{type.FullName}'.
         Have you registered it in the DI container !?
         """);