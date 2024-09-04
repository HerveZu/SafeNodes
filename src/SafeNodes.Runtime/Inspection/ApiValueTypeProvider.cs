using System.Reflection;
using SafeNodes.Internal.Reflection;

namespace SafeNodes.Runtime.Inspection;

internal sealed class ApiValueTypeProvider : IApiTypeProvider
{
    private readonly Assembly[] _assemblies = AppDomain.CurrentDomain.GetAssemblies();

    public ApiObjectAccess<Type> GetFromType(Type type)
    {
        return type.GetApiObjectAccess(type);
    }

    public IEnumerable<ApiObjectAccess<Type>> GetAssignableTo(Type targetType)
    {
        return GetAssignableToAny(targetType);
    }

    public IEnumerable<ApiObjectAccess<Type>> GetAssignableToAny(params Type[] targetTypes)
    {
        return _assemblies
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.HasApi())
            .Where(type => targetTypes.Any(type.IsAssignableToTypeOrGenericType))
            .Select(type => type.GetApiObjectAccess(type));
    }
}