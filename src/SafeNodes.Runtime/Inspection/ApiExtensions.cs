using System.Reflection;
using SafeNodes.Design;
using SafeNodes.Internal.Reflexion;

namespace SafeNodes.Runtime.Inspection;

internal static class ApiExtensions
{
    public static bool HasApi(this ICustomAttributeProvider provider)
    {
        return provider.HasAttribute<ApiAttribute>();
    }

    public static ApiObjectAccess<T> GetApiObjectAccess<T>(this ICustomAttributeProvider provider, T obj)
    {
        var api = provider.GetApi();

        return new ApiObjectAccess<T>(api.Reference, obj);
    }

    public static ApiObjectAccess<T> GetApiObjectAccess<T>(this T obj)
        where T : notnull
    {
        var api = obj.GetType().GetApi();

        return new ApiObjectAccess<T>(api.Reference, obj);
    }

    public static bool HasReference(this ICustomAttributeProvider provider, string reference)
    {
        return provider.HasApi() && provider.GetApi().Reference == reference;
    }

    public static IEnumerable<PropertyInfo> GetApiProperties<T>(
        this Type type,
        object item,
        BindingFlags bindingFlags)
    {
        return type
            .GetApiProperties(bindingFlags)
            .Where(p => p.GetValue(item) is T);
    }

    public static IEnumerable<PropertyInfo> GetApiProperties(
        this Type type,
        Type searchingType,
        BindingFlags bindingFlags)
    {
        return type
            .GetApiProperties(bindingFlags)
            .Where(p => p.PropertyType.IsAssignableToTypeOrGenericType(searchingType));
    }

    private static ApiAttribute GetApi(this ICustomAttributeProvider provider)
    {
        return provider.GetAttribute<ApiAttribute>();
    }

    private static IEnumerable<PropertyInfo> GetApiProperties(
        this IReflect type,
        BindingFlags bindingFlags)
    {
        return type
            .GetProperties(bindingFlags)
            .Where(p => p.HasApi());
    }
}