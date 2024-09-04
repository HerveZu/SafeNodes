using SafeNodes.Design;

namespace SafeNodes.Runtime.Context;

internal interface IBoundContextRegister<in TContext>
    where TContext : IBoundContext
{
    void Register(string reference, TContext context);
}