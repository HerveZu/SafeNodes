# SafeNodes
This package lets you easily create asynchronous & type safe node workflows (called blueprints). 
It consists in 3 main projects : 

- **SafeNodes.Design** : The contracts necessary to define your nodes system, such as nodes, events, types, etc... 

- **SafeNodes.Runtime** : The runtime that executes blueprints and generates schemes representing your nodes system definition.

- **SafeNodes.Internals** : A set of tools used internally.

> [!NOTE]
> By design, user facing contracts such as events, values or nodes must implement marker interfaces.
> This enforces contracts to be explicitly define and decoupled from logic.

## Getting started
A blueprint is made of at least two kind of items : events and nodes. 
Each blueprint requires exactly one event which is the entrypoint. 
The entrypoint nodes are the nodes triggered by the events which eventually trigger other nodes.
You likely want data to flow between events and nodes. To shape this data, custom values can be defined.

### Using SafeNodes assemblies
```csharp
// when defining contracts
using SafeNodes.Design; 

// when using the runtime
using SafeNodes.Runtime;
```

### Defining a value
```csharp
[Api("my-text-value")]
public sealed record TextValue(string Value) : IValue;
```

Values can define initializers, they create a new instance of the value from a raw string value. 
```csharp
[Api("my-trim-text-initializer")]
public sealed class TrimTextInitializer : IValueInitializer<TextValue>
{
    public TextValue InitializeValue(string rawValue)
    {
        return new TextValue(rawValue.Trim());
    }
}
```

### Defining an event
```csharp
// you could have as much data as you want here, I just keep things simple
public sealed record BlankData : IEventData;

[Api("my-blank-event")]
public sealed class BlankEvent : IEvent<BlankData>
{
    public void Define(BlankData data)
    {
    }

    public bool IsActivated() => true;
}
```

### Defining a node
```csharp
[Api("my-print-node")]
public sealed class PrintNode(IInput<TextValue> textToPrint, ITrigger done, IOutput<TextValue> textOutput) : INode
{
    [Api("print-text")]
    public IInput<TextValue> TextToPrint { get; } = textToPrint;
    
    [Api("text")]
    public IOutput<TextValue> TextOutput { get; } = textOutput;
    
    [Api("done")]
    public ITrigger Done { get; } = done;

    /// this is where the logic goes
    public async Task<ErrorOr<Success>> Execute(CancellationToken cancellationToken)
    {
        var textToPrint = TextToPrint.Get();
        Console.WriteLine(textToPrint);
        
        TextOutput.Set(textToPrint);
        
        // awaits for the children nodes (and their children) to complete
        await Done.Trigger(cancellationToken);

        return Result.Success;
    }
}
```

### Running a blueprint
```csharp
// setup a DI container
var builder = new ContainerBuilder();

// don't forget to add the SafeNodesModule in the DI container
builder.RegisterModule<SafeNodesModule>();

// your defined types need to be added to the DI container !! 
// the recommanded lifetime is Transicent (InstancePerDependency)
builder
    .RegisterTypes([
        typeof(BlankEvent),
        typeof(PrintNode),
        typeof(TrimTextInitializer)
    ])
    .AsImplementedInterfaces()
    .InstancePerDependency();

var app = builder.Build();

await using var scope = app.BeginLifetimeScope();

var blueprintRuntime = scope.Resolve<IBlueprintRuntime>();
Console.WriteLine("Runtime ...");

// this is your blueprint definition. Use the API references in it.
var blueprint = new Blueprint
{
    Event = new BlueprintEvent
    {
        EventReference = "my-blank-event"
    },
    Nodes =
    [
        new BlueprintNode
        {
            // this id is a runtime id, 
            // this allows to have multiple nodes of the same types
            Id = "node-1",
            NodeReference = "my-print-node",
            IsEntrypoint = true, // this node is triggered by the event
            Inputs =
            [
                new BlueprintNodeInput
                {
                    InputReference = "print-text",
                    
                    // set the input value from a raw value and an initializer
                    Initializer = new BlueprintNodeInputInitializer
                    {
                        InitializerReference = "my-trim-text-initializer",
                        RawValue = " Hello, World ! "
                    }
                }
            ],
        },
        new BlueprintNode
        {
            Id = "node-2",
            NodeReference = "my-print-node",
            Inputs =
            [
                new BlueprintNodeInput
                {
                    InputReference = "print-text",
                    // this is the value to set to input to,
                    // it comes from the output 'text' of the node 'node-id'
                    Source = new BlueprintNodeInputSource
                    {
                        NodeId = "node-1",
                        OutputReference = "text"
                    }
                }
            ],
            // this node is not triggered by the event, 
            // rather by the 'done' trigger of the 'node-1' node
            Trigger = new BlueprintNodeTrigger
            {
                NodeId = "node-1",
                TriggerReference = "done"
            }
        }
    ]
};

// execute the blueprint, throws if the event data is not compatible with the blueprint's event
await blueprintRuntime.ExecuteMandatory(blueprint, new BlankData());

// execute the blueprint or skip when the event data is not compatible
// var blueprintWasExecuted = await blueprintRuntime.Execute(blueprint, new BlankData());
```

## TODO
The code was important from an existing project, thus misses important parts
- [x] Getting started
- [ ] Publish on nugget.org
- [ ] Documentation
- [ ] Unit tests
- [ ] Blueprint validation