using JetBrains.Annotations;
using SafeNodes.Design;
using SafeNodes.Runtime.Execution;

namespace SafeNodes.Runtime;

/// <summary>
/// The blueprint runtime, where blueprint are executed is isolated environments.
/// </summary>
[PublicAPI]
public interface IBlueprintRuntime
{
    /// <summary>
    /// Execute a blueprint in its DI scope.
    /// If the <see cref="eventData"/> doesn't match the <see cref="blueprint"/>'s event,
    /// <see cref="MismatchingEventDataException"/> is thrown.
    /// </summary>
    /// <param name="blueprint">The blueprint to execute</param>
    /// <param name="eventData">The event data corresponding the blueprint's event</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>The execution task</returns>
    Task ExecuteMandatory(Blueprint blueprint, IEventData eventData, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Execute a blueprint in its DI scope if the event data matches the blueprint's event.
    /// </summary>
    /// <param name="blueprint">The blueprint to execute</param>
    /// <param name="eventData">Some event data</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>True if the blueprint has been executed, otherwise false</returns>
    Task<bool> Execute(Blueprint blueprint, IEventData eventData, CancellationToken cancellationToken = default);
}