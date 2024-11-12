using JetBrains.Annotations;

namespace SafeNodes.Design;

/// <summary>
/// Define the underlying element as part of the nodes system API, which can be referenced.
/// </summary>
/// <param name="reference">The locally unique reference. Make sure to use a stable value.</param>
/// <param name="docs">Publicly available documentation</param>
[PublicAPI]
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Property)]
[MeansImplicitUse]
public sealed class ApiAttribute(string reference, string? docs = null) : Attribute
{
    /// <summary>
    /// The API member reference.
    /// </summary>
    public string Reference { get; } = reference;
    
    /// <summary>
    /// Publicly available documentation
    /// </summary>
    public string? Docs { get; } = docs;
}