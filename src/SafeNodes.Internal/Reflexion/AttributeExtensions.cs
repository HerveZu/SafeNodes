using System.Reflection;

namespace SafeNodes.Internal.Reflexion;

public static class AttributeExtensions
{
    public static TAttribute GetAttribute<TAttribute>(this ICustomAttributeProvider provider)
    {
        return provider.GetCustomAttributes<TAttribute>().Last();
    }

    public static TAttribute? GetAttributeOrDefault<TAttribute>(this ICustomAttributeProvider provider)
    {
        return provider.GetCustomAttributes<TAttribute>().LastOrDefault();
    }

    public static TAttribute[] GetCustomAttributes<TAttribute>(this ICustomAttributeProvider provider)
    {
        return provider
            .GetCustomAttributes(typeof(TAttribute), true)
            .Cast<TAttribute>()
            .ToArray();
    }

    public static bool HasAttribute<TAttribute>(this ICustomAttributeProvider provider)
    {
        var customAttributes = provider.GetCustomAttributes(typeof(TAttribute), true);

        return customAttributes.Length is not 0;
    }
}