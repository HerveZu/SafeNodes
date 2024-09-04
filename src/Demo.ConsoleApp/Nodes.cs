using ErrorOr;
using SafeNodes.Design;
using SafeNodes.Runtime;

namespace Demo.ConsoleApp;

public sealed class LogNodeExecution : INodeContextPipeline
{
    public async Task<IErrorOr> Next(INode node, NodeContextPipelineNext next, CancellationToken cancellationToken)
    {
        Console.WriteLine($"---> {node.GetType()}");

        var result = await next();
        
        Console.WriteLine($"<--- {node.GetType()}");
        
        return result;
    }
}

[Api("text")]
public sealed record TextValue(string Value) : IValue;

[Api("text-initializer")]
public sealed class TextInitializer : IValueInitializer<TextValue>
{
    public TextValue InitializeValue(string rawValue)
    {
        return new TextValue(rawValue);
    }
}

public sealed record BlankData : IEventData;

[Api("blank-event")]
public sealed class BlankEvent : IEvent<BlankData>
{
    public void Define(BlankData data)
    {
    }

    public bool IsActivated() => true;
}

[Api("print-node")]
public sealed class PrintNode(IInput<TextValue> text, ITrigger done, IOutput<TextValue> textOutput) : INode
{
    [Api("text")]
    public IInput<TextValue> Text { get; } = text;
    
    [Api("text")]
    public IOutput<TextValue> TextOutput { get; } = textOutput;
    
    [Api("done")]
    public ITrigger Done { get; } = done;

    public async Task<ErrorOr<Success>> Execute(CancellationToken cancellationToken)
    {
        var text = Text.Get();
        Console.WriteLine(text);
        
        TextOutput.Set(text);
        
        await Done.Trigger(cancellationToken);

        return Result.Success;
    }
}