namespace SafeNodes.Runtime.Inspection;

internal interface IApiServiceProvider
{
    ApiObjectAccess<T> GetByReference<T>(string reference)
        where T : notnull;

    ApiObjectAccess<object>? GetByReferenceAssignableTo(Type targetType, string reference);
}