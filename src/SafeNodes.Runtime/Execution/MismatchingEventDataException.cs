using SafeNodes.Design;

namespace SafeNodes.Runtime.Execution;

/// <summary>
/// Thrown when <see cref="IEventData"/> doesn't match <see cref="Blueprint"/>'s event.
/// </summary>
/// <param name="eventDataType">The event data type</param>
/// <param name="eventReference">The blueprint's event reference</param>
public sealed class MismatchingEventDataException(Type eventDataType, string eventReference)
    : InvalidOperationException(
        $"The event with reference '{eventReference}' doesn't expect '{eventDataType.FullName}'");