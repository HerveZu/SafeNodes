using Autofac;

namespace SafeNodes.Runtime.Schemes;

internal sealed class SchemeProviderProxy(ILifetimeScope lifetimeScope) : ISchemeProvider
{
    public IEnumerable<TScheme> GetSchemes<TScheme>() where TScheme : IScheme
    {
        return lifetimeScope.Resolve<ISchemeProvider<TScheme>>().GetSchemes();
    }
}