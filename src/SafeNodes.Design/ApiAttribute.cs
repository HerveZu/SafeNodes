using JetBrains.Annotations;

namespace SafeNodes.Design;

[PublicAPI]
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
[MeansImplicitUse]
public sealed class ApiAttribute(string reference) : Attribute
{
    public string Reference { get; } = reference;
}