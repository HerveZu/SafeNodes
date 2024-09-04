using Autofac;
using SafeNodes.Runtime.Context;

namespace SafeNodes.Runtime.Inspection;

internal sealed class ApiServiceProvider(
    ILifetimeScope lifetimeScope,
    IPropertyAccessor propertyAccessor,
    IContextBinder contextBinder
)
    : IApiServiceProvider
{
    public ApiObjectAccess<T> GetByReference<T>(string reference)
        where T : notnull
    {
        var service = lifetimeScope
            .Resolve<IEnumerable<T>>()
            .FirstOrDefault(service => service.GetType().HasReference(reference));

        if (service is null)
        {
            throw new InvalidApiReferenceException(reference, typeof(T));
        }

        BindPropertiesApiContext(service);

        return service.GetApiObjectAccess();
    }

    public ApiObjectAccess<object> GetByReferenceAssignableTo(Type targetType, string reference)
    {
        var servicesType = typeof(IEnumerable<>).MakeGenericType(targetType);
        var resolvedServices = (IEnumerable<object>)lifetimeScope.Resolve(servicesType);

        var services = resolvedServices
            .Where(service => service.GetType().HasApi())
            .ToArray();

        foreach (var service in services)
        {
            BindPropertiesApiContext(service);
        }

        return services
                   .Select(service => service.GetApiObjectAccess())
                   .FirstOrDefault(service => service.Reference == reference)
               ?? throw new InvalidApiReferenceException(reference, targetType);
    }

    private void BindPropertiesApiContext(object service)
    {
        var propertiesAccesses = propertyAccessor.GetValues<IBindContext>(service);

        foreach (var access in propertiesAccesses)
        {
            contextBinder.Bind(access.Reference, access.Object);
        }
    }
}