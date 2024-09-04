using JetBrains.Annotations;

namespace SafeNodes.Design;

[PublicAPI]
public interface IEventData;

[PublicAPI]
public interface IEventBone
{
    bool IsActivated();
}

[PublicAPI]
public interface IEvent<in TData> : IEventBone
    where TData : IEventData
{
    void Define(TData data);
}