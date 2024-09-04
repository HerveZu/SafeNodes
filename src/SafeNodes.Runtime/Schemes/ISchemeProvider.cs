using JetBrains.Annotations;

namespace SafeNodes.Runtime.Schemes;

[PublicAPI]
public interface IScheme;

[PublicAPI]
public interface ISchemeProvider
{
    IEnumerable<TScheme> GetSchemes<TScheme>()
        where TScheme : IScheme;
}

internal interface ISchemeProvider<out TScheme>
    where TScheme : IScheme
{
    IEnumerable<TScheme> GetSchemes();
}