using System.Text.Json;
using Autofac;
using Demo.ConsoleApp;
using SafeNodes.Runtime;
using SafeNodes.Runtime.Schemes;

var builder = new ContainerBuilder();
builder.RegisterModule<SafeNodesModule>();

builder
    .RegisterTypes([
        typeof(BlankEvent),
        typeof(PrintNode),
        typeof(TextInitializer),
        typeof(LogNodeExecution)
    ])
    .AsImplementedInterfaces()
    .InstancePerDependency();

var app = builder.Build();

await using var scope = app.BeginLifetimeScope();

var blueprintRuntime = scope.Resolve<IBlueprintRuntime>();
var schemeProvider = scope.Resolve<ISchemeProvider>();

var jsonOptions = new JsonSerializerOptions
{
    WriteIndented = true
};

Console.WriteLine("Schemes ...");

Console.WriteLine(JsonSerializer.Serialize(schemeProvider.GetSchemes<NodeScheme>(), jsonOptions));
Console.WriteLine(JsonSerializer.Serialize(schemeProvider.GetSchemes<TypeScheme>(), jsonOptions));
Console.WriteLine(JsonSerializer.Serialize(schemeProvider.GetSchemes<InitializerScheme>(), jsonOptions));
Console.WriteLine(JsonSerializer.Serialize(schemeProvider.GetSchemes<EventScheme>(), jsonOptions));

Console.WriteLine("Runtime ...");

var blueprint = new Blueprint
{
    Event = new BlueprintEvent
    {
        EventReference = "blank-event",
        PropertyActivations = []
    },
    Nodes =
    [
        new BlueprintNode
        {
            Id = "node-1",
            NodeReference = "print-node",
            IsEntrypoint = true,
            Inputs =
            [
                new BlueprintNodeInput
                {
                    InputReference = "text",
                    Initializer = new BlueprintNodeInputInitializer
                    {
                        InitializerReference = "text-initializer",
                        RawValue = "Hello, World !"
                    }
                }
            ],
        },
        new BlueprintNode
        {
            Id = "node-2",
            NodeReference = "print-node",
            Inputs =
            [
                new BlueprintNodeInput
                {
                    InputReference = "text",
                    Source = new BlueprintNodeInputSource
                    {
                        NodeId = "node-1",
                        OutputReference = "text"
                    }
                }
            ],
            Trigger = new BlueprintNodeTrigger
            {
                NodeId = "node-1",
                TriggerReference = "done"
            }
        }
    ]
};

await blueprintRuntime.Execute(blueprint, new BlankData());