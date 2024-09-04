namespace SafeNodes.Runtime.Execution;

internal interface INodeEventNotifier
{
    void OnOutputEmitted(OutputEmittedCallback callback);
    void OnTriggered(NodeTriggeredCallback callback);
}