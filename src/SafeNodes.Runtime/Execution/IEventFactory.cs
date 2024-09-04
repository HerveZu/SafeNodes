using SafeNodes.Design;

namespace SafeNodes.Runtime.Execution;

internal interface IEventFactory
{
    IEventBone Create(string reference, IEventData data);
}