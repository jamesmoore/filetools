/*
 * Example invocation:
 * FileToolsCli.exe duplicate --type WholeFile --path "C:\Users\user\Documents" "C:\Users\user\Downloads"
 */

using System.CommandLine;
using System.IO.Abstractions;
using FileTools.Common;
using FileTools.Compressed;
using FileTools.Dupecheck;
using FileTools.HashStore;
using FileTools.PDFHasher;

var typeOption = new Option<string>("--type")
{
    Description = "How to compare files.",
    DefaultValueFactory = _ => nameof(FileProcessingMethod.WholeFile),
};
typeOption.AcceptOnlyFromAmong(
    StringComparer.OrdinalIgnoreCase,
    nameof(FileProcessingMethod.WholeFile),
    nameof(FileProcessingMethod.Contents),
    nameof(FileProcessingMethod.Metadata));

var pathOption = new Option<string[]>("--path")
{
    Description = "One or more directories to scan recursively.",
    Required = true,
    Arity = ArgumentArity.OneOrMore,
    AllowMultipleArgumentsPerToken = true,
};
pathOption.Validators.Add(result =>
{
    foreach (var path in result.GetValueOrDefault<string[]>() ?? [])
    {
        if (!Directory.Exists(path))
            result.AddError($"Directory does not exist or cannot be accessed: {path}");
    }
});

var duplicateCommand = new Command("duplicate", "Find duplicate files.");
duplicateCommand.Options.Add(typeOption);
duplicateCommand.Options.Add(pathOption);
duplicateCommand.SetAction(parseResult => ListDuplicates(
    Enum.Parse<FileProcessingMethod>(parseResult.GetValue(typeOption)!, ignoreCase: true),
    parseResult.GetValue(pathOption)!));

var root = new RootCommand("FileTools command-line utilities.");
root.Subcommands.Add(duplicateCommand);
return await root.Parse(args).InvokeAsync();

static async Task ListDuplicates(FileProcessingMethod method, string[] paths)
{
    using var progress = new ConsoleDuplicateFinderObserver();
    progress.SetProgressPercent(0);

    var fileSystem = new FileSystem();
    var files = new DuplicateRecurser();
    var recurser = new DirectoryRecurser(files);
    foreach (var path in paths)
        recurser.Recurse(fileSystem.DirectoryInfo.New(path));

    using var hashStore = new HashStoreFactory(fileSystem).GetHashStore();
    var finder = new DuplicateFinder(
        method,
        progress,
        new SharpCompressWrapper(fileSystem),
        fileSystem,
        new GhostScriptApi(new GhostScriptLocator(fileSystem)),
        hashStore,
        matchFileName: false);

    var duplicates = await finder.Process(files.FileList.Values);
    progress.Complete();
    var groups = duplicates.ToLookup(item => item.Hash);
    foreach (var group in groups.OrderBy(group => group.Min(item => item.FileItem.Length)))
    {
        var sorted = group.OrderBy(item => item.FileItem.Length).ToList();
        var minSize = sorted[0].FileItem.Length;
        var maxSize = sorted[^1].FileItem.Length;
        var size = minSize == maxSize ? $"{minSize:N0} bytes" : $"{minSize:N0}–{maxSize:N0} bytes";
        Console.WriteLine($"{group.Key} ({size})");
        foreach (var item in sorted)
            Console.WriteLine($"  {item.FileItem.FullName}");
    }

    Console.WriteLine($"{duplicates.Count} duplicated files in {groups.Count} groups.");
}

sealed class ConsoleDuplicateFinderObserver : IDuplicateFinderObserver, IDisposable
{
    private readonly bool isErrorRedirected = Console.IsErrorRedirected;
    private int lastPercent = -1;
    private bool hasOpenLine;

    public void SetProgressText(string file) { }

    public void SetProgressPercent(int percent)
    {
        // The finder reports 100% after hashing, before it groups the results.
        if (percent >= 100 || percent <= lastPercent)
            return;

        if (isErrorRedirected && lastPercent >= 0 && percent / 10 <= lastPercent / 10)
            return;

        var message = $"Duplicate scan: {percent,3}%";
        if (isErrorRedirected)
        {
            Console.Error.WriteLine(message);
        }
        else
        {
            Console.Error.Write($"\r{message}");
            hasOpenLine = true;
        }

        lastPercent = percent;
    }

    public void Complete()
    {
        if (!isErrorRedirected)
            Console.Error.Write('\r');

        Console.Error.WriteLine("Duplicate scan: 100%");
        hasOpenLine = false;
    }

    public void Dispose()
    {
        if (hasOpenLine)
            Console.Error.WriteLine();
    }
}
