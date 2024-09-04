using ErrorOr;
using SafeNodes.Design;
using SafeNodes.Runtime;

namespace Demo.WatchFolder;

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

[Api("local-file")]
public sealed record LocalFile : IValue
{
    [Api("file-path")]
    public required TextValue FilePath { get; init; }
}

public sealed record StartupData : IEventData;

[Api("startup")]
public sealed class StartupEvent : IEvent<StartupData>
{
    public void Define(StartupData data)
    {
    }

    public bool IsActivated() => true;
}

[Api("print")]
public sealed class PrintNode(IInput<TextValue> textToPrint) : INode
{
    [Api("text")]
    public IInput<TextValue> TextToPrint { get; } = textToPrint;

    public Task<ErrorOr<Success>> Execute(CancellationToken cancellationToken)
    {
        var textToPrint = TextToPrint.Get();
        Console.WriteLine(textToPrint);

        return Task.FromResult<ErrorOr<Success>>(Result.Success);
    }
}

[Api("watch-folder")]
public sealed class WatchFolderNode(IInput<TextValue> folderPath, IOutput<LocalFile> changedFile, ITrigger fileChanged)
    : INode
{
    [Api("folder-path")]
    public IInput<TextValue> FolderPath { get; } = folderPath;

    [Api("file")]
    public IOutput<LocalFile> ChangedFile { get; } = changedFile;

    [Api("file-changed")]
    public ITrigger FileChanged { get; } = fileChanged;

    public async Task<ErrorOr<Success>> Execute(CancellationToken cancellationToken)
    {
        var folderPath = FolderPath.Get().Value;
        Console.WriteLine($"Watching {folderPath}");

        var watcher = new FileSystemWatcher();

        watcher.Path = folderPath;
        watcher.NotifyFilter = NotifyFilters.LastWrite;
        watcher.Filter = "*.*";
        watcher.EnableRaisingEvents = true;

        watcher.Changed += async (_, e) =>
        {
            ChangedFile.Set(new LocalFile
            {
                FilePath = new TextValue(e.FullPath)
            });
            await FileChanged.Trigger(cancellationToken);
        };

        await Task.Delay(-1, cancellationToken);

        return Result.Success;
    }
}