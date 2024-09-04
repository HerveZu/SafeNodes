using Autofac;
using JetBrains.Annotations;
using SafeNodes.Design;
using SafeNodes.Runtime.Context;
using SafeNodes.Runtime.Execution;
using SafeNodes.Runtime.Inspection;
using SafeNodes.Runtime.Nodes;
using SafeNodes.Runtime.Schemes;

namespace SafeNodes.Runtime;

[PublicAPI]
public sealed class SafeNodesModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder
            .RegisterGeneric(typeof(Output<>))
            .As(typeof(IOutput<>))
            .InstancePerDependency();
        
        builder
            .RegisterGeneric(typeof(NullableInput<>))
            .As(typeof(INullableInput<>))
            .InstancePerDependency();
        
        builder
            .RegisterGeneric(typeof(NonNullableInput<>))
            .As(typeof(IInput<>))
            .InstancePerDependency();
        
        builder
            .RegisterGeneric(typeof(EventActivation<>))
            .As(typeof(IEventActivation<>))
            .InstancePerDependency();
        
        builder
            .RegisterGeneric(typeof(EventValue<>))
            .As(typeof(IEventValue<>))
            .InstancePerDependency();
        
        builder
            .RegisterTypes([
                typeof(ApiContextService),
                typeof(ApiServiceProvider),
                typeof(ApiValueTypeProvider),
                typeof(PropertyAccessor),
                typeof(ContextServiceProxy),
                typeof(EventFactory),
                
                typeof(EnvironmentValueProvider),
                typeof(NodeContextStackExecutionPipeline),
                typeof(NodeExecutor),
                typeof(BlueprintRuntime),
                
                typeof(InitializerSchemeProvider),
                typeof(EventSchemeProvider),
                typeof(NodeSchemeProvider),
                typeof(TypeSchemeProvider),
                typeof(SchemeProviderProxy),
                
                typeof(ExecutionTrigger)
            ])
            .AsImplementedInterfaces()
            .InstancePerDependency();

        builder
            .RegisterGeneric(typeof(ScopedContextService<>))
            .AsImplementedInterfaces()
            .InstancePerMatchingLifetimeScope(LifetimeScopes.BlueprintExecution);

        builder
            .RegisterType<ExecutionNodeEventNotifier>()
            .AsImplementedInterfaces()
            .InstancePerMatchingLifetimeScope(LifetimeScopes.BlueprintExecution);

        builder
            .RegisterType<ContextBindingService>()
            .AsImplementedInterfaces()
            .InstancePerMatchingLifetimeScope(LifetimeScopes.BlueprintExecution);

        builder
            .RegisterGeneric(typeof(BoundContextService<>))
            .AsImplementedInterfaces()
            .InstancePerMatchingLifetimeScope(LifetimeScopes.NodeExecution);

        builder
            .RegisterType<ScopedContextService<NodeExecutionContext>>()
            .AsImplementedInterfaces()
            .InstancePerMatchingLifetimeScope(LifetimeScopes.NodeExecution);
    }
}