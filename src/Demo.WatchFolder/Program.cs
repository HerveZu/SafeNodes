using System.Text.Json;
using Autofac;
using Demo.WatchFolder;
using SafeNodes.Runtime;
using SafeNodes.Runtime.Schemes;

var builder = new ContainerBuilder();
builder.RegisterModule<SafeNodesModule>();

builder
    .RegisterTypes([
        typeof(StartupEvent),
        typeof(WatchFolderNode),
        typeof(PrintNode),
        typeof(TextInitializer)
    ])
    .AsImplementedInterfaces()
    .InstancePerDependency();

builder
    .RegisterGeneric(typeof(LogNodeExecution<>))
    .AsImplementedInterfaces()
    .InstancePerDependency();

builder
    .RegisterGeneric(typeof(BenchmarkNodes<>))
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
        EventReference = "startup"
    },
    Nodes =
    [
        new BlueprintNode
        {
            Id = "node-1",
            NodeReference = "watch-folder",
            IsEntrypoint = true,
            Inputs =
            [
                new BlueprintNodeInput
                {
                    InputReference = "folder-path",
                    Initializer = new BlueprintNodeInputInitializer
                    {
                        InitializerReference = "text-initializer",
                        RawValue = "C:\\Temp"
                    }
                }
            ],
        },
        new BlueprintNode
        {
            Id = "node-2",
            NodeReference = "print",
            Inputs =
            [
                new BlueprintNodeInput
                {
                    InputReference = "text",
                    Source = new BlueprintNodeInputSource
                    {
                        NodeId = "node-1",
                        OutputReference = "file",
                        Properties = ["file-path"]
                    }
                }
            ],
            Trigger = new BlueprintNodeTrigger
            {
                NodeId = "node-1",
                TriggerReference = "file-changed"
            }
        }
    ]
};

await blueprintRuntime.ExecuteMandatory(blueprint, new StartupData());