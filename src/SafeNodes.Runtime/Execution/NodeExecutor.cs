using Autofac;
using SafeNodes.Design;
using SafeNodes.Runtime.Context;
using SafeNodes.Runtime.Inspection;
using SafeNodes.Runtime.Nodes;

namespace SafeNodes.Runtime.Execution;

internal sealed class NodeExecutor(ILifetimeScope lifetimeScope)
    : INodeExecutor, IAsyncDisposable
{
    private readonly ILifetimeScope _blueprintExecutionScope
        = lifetimeScope.BeginLifetimeScope(LifetimeScopes.BlueprintExecution);

    public async ValueTask DisposeAsync()
    {
        await _blueprintExecutionScope.DisposeAsync();
        await lifetimeScope.DisposeAsync();
    }

    public void OnNodeOutputEmitted(OutputEmittedCallback callback)
    {
        _blueprintExecutionScope
            .Resolve<INodeEventNotifier>()
            .OnOutputEmitted(callback);
    }

    public void OnNodeTriggered(NodeTriggeredCallback callback)
    {
        _blueprintExecutionScope
            .Resolve<INodeEventNotifier>()
            .OnTriggered(callback);
    }

    public IEventBone? InitializeEvent(Blueprint blueprint, IEventData eventData)
    {
        var contextRegister = _blueprintExecutionScope.Resolve<IContextRegister>();
        var eventFactory = _blueprintExecutionScope.Resolve<IEventFactory>();

        var @event = eventFactory.Create(blueprint.Event.EventReference, eventData);

        if (@event is null)
        {
            return null;
        }
        
        contextRegister.RegisterScoped(
            new GlobalExecutionContext
            {
                Event = @event
            });

        contextRegister.RegisterScoped(
            new EventActivationContext
            {
                EventActivations = blueprint.Event.PropertyActivations
            });

        return @event;
    }

    public async Task ExecuteNode(
        BlueprintNode node,
        IReadOnlyEnvironment environment,
        CancellationToken cancellationToken)
    {
        var nodeExecutionScope = _blueprintExecutionScope.BeginLifetimeScope(LifetimeScopes.NodeExecution);

        var apiServiceProvider = nodeExecutionScope.Resolve<IApiServiceProvider>();
        var contextRegister = nodeExecutionScope.Resolve<IContextRegister>();
        var executionPipeline = nodeExecutionScope.Resolve<INodeContextExecutionPipeline>();

        contextRegister.RegisterScoped(
            new NodeExecutionContext
            {
                NodeId = node.Id,
                Environment = environment
            });

        var inputs = node.Inputs.ToArray();

        foreach (var input in inputs.Where(input => input.Initializer is not null))
        {
            contextRegister.RegisterBound(
                input.InputReference,
                new InitializerContext
                {
                    Reference = input.Initializer!.InitializerReference,
                    Value = input.Initializer.RawValue
                });
        }

        foreach (var input in inputs.Where(input => input.Source is not null))
        {
            contextRegister.RegisterBound(
                input.InputReference,
                new SourceContext
                {
                    NodeId = input.Source!.NodeId,
                    OutputReference = input.Source.OutputReference,
                    Properties = input.Source.Properties
                });
        }

        var nodeAccess = apiServiceProvider.GetByReference<INode>(node.NodeReference);
        await executionPipeline.Execute(nodeAccess.Object, cancellationToken);
    }
}