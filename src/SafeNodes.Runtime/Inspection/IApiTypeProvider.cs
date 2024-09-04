namespace SafeNodes.Runtime.Inspection;

internal interface IApiTypeProvider
{
    ApiObjectAccess<Type> GetFromType(Type type);
    IEnumerable<ApiObjectAccess<Type>> GetAssignableTo(Type targetType);
    IEnumerable<ApiObjectAccess<Type>> GetAssignableToAny(params Type[] targetTypes);
}