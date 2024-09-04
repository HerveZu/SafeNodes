namespace SafeNodes.Internal.Reflection;

public static class TypeExtensions
{
    public static bool IsAssignableToGenericType(this Type givenType, Type genericType)
    {
        var interfaceTypes = givenType.GetInterfaces();

        if (interfaceTypes.Any(it => it.IsGenericType && it.GetGenericTypeDefinition() == genericType))
        {
            return true;
        }

        if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
        {
            return true;
        }

        var baseType = givenType.BaseType;

        return baseType != null && IsAssignableToGenericType(baseType, genericType);
    }

    public static bool IsAssignableToTypeOrGenericType(this Type givenType, Type genericType)
    {
        return givenType.IsAssignableTo(genericType) || givenType.IsAssignableToGenericType(genericType);
    }

    public static bool IsConcreteAndAssignableToGenericType(this Type givenType, Type genericType)
    {
        return givenType.IsConcrete() && givenType.IsAssignableToGenericType(genericType);
    }

    public static bool IsConcreteAndAssignableTo(this Type givenType, Type genericType)
    {
        return givenType.IsConcrete() && givenType.IsAssignableTo(genericType);
    }

    public static IEnumerable<Type> GetGenericsFromInterface(this Type type, Type interfaceType)
    {
        return type.GetInterface(interfaceType.Name)!.GetGenericArguments();
    }

    private static bool IsConcrete(this Type type)
    {
        return type is { IsInterface: false, IsAbstract: false };
    }
}