namespace SafeNodes.Runtime.Inspection;

internal interface IPropertyAccessor
{
    IEnumerable<ApiObjectAccess<T>> GetValues<T>(object obj);
    IEnumerable<ApiObjectAccess<Type>> GetPropertiesType(Type objectType, Type searchingType);
}