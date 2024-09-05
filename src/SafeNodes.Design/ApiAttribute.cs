using JetBrains.Annotations;

namespace SafeNodes.Design;

/// <summary>
/// Define the underlying element as part of the nodes system API, which can be referenced.
/// </summary>
/// <param name="reference">The locally unique reference. Make sure to use a stable value.</param>
[PublicAPI]
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
[MeansImplicitUse]
public sealed class ApiAttribute(string reference) : Attribute
{
    /// <summary>
    /// The API member reference.
    /// </summary>
    public string Reference { get; } = reference;
}