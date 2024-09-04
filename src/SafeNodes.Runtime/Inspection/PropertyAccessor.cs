using System.Reflection;

namespace SafeNodes.Runtime.Inspection;

internal sealed class PropertyAccessor : IPropertyAccessor
{
    private const BindingFlags PropertiesBindingFlags =
        BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.NonPublic | BindingFlags.Public;

    public IEnumerable<ApiObjectAccess<T>> GetValues<T>(object obj)
    {
        var type = obj.GetType();

        return type
            .GetApiProperties<T>(obj, PropertiesBindingFlags)
            .Select(info => info.GetApiObjectAccess<T>((T)info.GetValue(obj)!));
    }

    public IEnumerable<ApiObjectAccess<Type>> GetPropertiesType(Type objectType, Type searchingType)
    {
        return objectType
            .GetApiProperties(searchingType, PropertiesBindingFlags)
            .Select(info => info.GetApiObjectAccess(info.PropertyType));
    }
}