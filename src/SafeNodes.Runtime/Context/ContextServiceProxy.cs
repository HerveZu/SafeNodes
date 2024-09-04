using Autofac;
using SafeNodes.Design;

namespace SafeNodes.Runtime.Context;

internal sealed class ContextServiceProxy(ILifetimeScope lifetimeScope) : IContextResolver, IContextRegister
{
    public void RegisterScoped<TContext>(TContext context)
        where TContext : IScopedContext
    {
        var service = lifetimeScope.Resolve<IScopedContextRegister<TContext>>();

        service.Register(context);
    }

    public void RegisterBound<TContext>(string reference, TContext context)
        where TContext : IBoundContext
    {
        var service = lifetimeScope.Resolve<IBoundContextRegister<TContext>>();

        service.Register(reference, context);
    }

    public TContext Scoped<TContext>()
        where TContext : IScopedContext
    {
        var resolver = lifetimeScope.Resolve<IScopedContextResolver<TContext>>();

        return resolver.Resolve();
    }

    public TContext Bound<TContext>(IBindContext requester)
        where TContext : IBoundContext
    {
        var resolver = lifetimeScope.Resolve<IBoundContextResolver<TContext>>();

        return resolver.Resolve(requester);
    }

    public TContext? BoundOrDefault<TContext>(IBindContext requester)
        where TContext : IBoundContext
    {
        var service = lifetimeScope.Resolve<IBoundContextResolver<TContext>>();

        return service.ResolveOrDefault(requester);
    }
}