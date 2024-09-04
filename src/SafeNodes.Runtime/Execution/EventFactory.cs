using SafeNodes.Design;
using SafeNodes.Runtime.Inspection;

namespace SafeNodes.Runtime.Execution;

internal sealed class EventFactory(IApiServiceProvider apiServiceProvider) : IEventFactory
{
    public IEventBone? Create(string reference, IEventData data)
    {
        var eventType = typeof(IEvent<>).MakeGenericType(data.GetType());
        var access = apiServiceProvider.GetByReferenceAssignableTo(eventType, reference);

        if (access is null)
        {
            return null;
        }
        
        var castedEvent = (IEventBone)access.Object;

        DefineEvent(castedEvent, data);

        return castedEvent;
    }

    private static void DefineEvent(IEventBone @event, IEventData data)
    {
        var define = @event.GetType().GetMethod(nameof(IEvent<IEventData>.Define))!;
        define.Invoke(@event, [data]);
    }
}