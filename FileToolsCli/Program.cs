/*
 * Example invocation:
 * FileToolsCli.exe duplicate --type WholeFile --path "C:\Users\user\Documents" "C:\Users\user\Downloads"
 */

using System.CommandLine;
using System.IO.Abstractions;
using System.IO.Enumeration;
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

var deleteOption = new Option<string?>("--delete")
{
    Description = "Preview and confirm deletion of WholeFile duplicates matching a full-path wildcard (* or ?).",
    Arity = ArgumentArity.ExactlyOne,
};

var duplicateCommand = new Command("duplicate", "Find duplicate files.");
duplicateCommand.Options.Add(typeOption);
duplicateCommand.Options.Add(pathOption);
duplicateCommand.Options.Add(deleteOption);
duplicateCommand.SetAction(parseResult => ListDuplicates(
    Enum.Parse<FileProcessingMethod>(parseResult.GetValue(typeOption)!, ignoreCase: true),
    parseResult.GetValue(pathOption)!,
    parseResult.GetValue(deleteOption)));

var root = new RootCommand("FileTools command-line utilities.");
root.Subcommands.Add(duplicateCommand);
return await root.Parse(args).InvokeAsync();

static async Task<int> ListDuplicates(FileProcessingMethod method, string[] paths, string? deletePattern)
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
    var candidates = new List<DuplicateFileItem>();
    var skippedGroups = 0;
    var pattern = deletePattern is null ? null : NormalizePath(deletePattern);
    foreach (var group in groups.OrderBy(group => group.Min(item => item.FileItem.Length)))
    {
        var sorted = group.OrderBy(item => item.FileItem.Length).ToList();
        var minSize = sorted[0].FileItem.Length;
        var maxSize = sorted[^1].FileItem.Length;
        var size = minSize == maxSize ? $"{minSize:N0} bytes" : $"{minSize:N0}–{maxSize:N0} bytes";
        Console.WriteLine($"{group.Key} ({size})");
        var matches = pattern is null ? [] : sorted.Where(item => FileSystemName.MatchesSimpleExpression(
            pattern, NormalizePath(item.FileItem.FullName), ignoreCase: OperatingSystem.IsWindows())).ToHashSet();
        if (matches.Count == sorted.Count)
        {
            Console.WriteLine("  Skipping deletion for this group because every file matches.");
            matches.Clear();
            skippedGroups++;
        }

        foreach (var item in sorted)
        {
            var candidate = matches.Contains(item);
            if (candidate)
                candidates.Add(item);

            WritePath(item.FileItem.FullName, candidate);
        }
    }

    Console.WriteLine($"{duplicates.Count} duplicated files in {groups.Count} groups.");
    if (pattern is null)
        return 0;

    if (skippedGroups > 0)
        Console.WriteLine($"Skipped {skippedGroups} groups where every file matched.");

    if (candidates.Count == 0)
    {
        Console.WriteLine("No files eligible for deletion matched the pattern.");
        return 0;
    }

    Console.WriteLine($"\nFiles proposed for deletion ({candidates.Count}):");
    foreach (var candidate in candidates)
        WritePath(candidate.FileItem.FullName, highlight: false);

    if (Console.IsInputRedirected)
    {
        Console.Error.WriteLine("Deletion requires confirmation in an interactive terminal. No files deleted.");
        return 1;
    }

    Console.Error.Write("Delete these files? [y/N] ");
    var answer = Console.ReadLine()?.Trim();
    if (!string.Equals(answer, "y", StringComparison.OrdinalIgnoreCase) &&
        !string.Equals(answer, "yes", StringComparison.OrdinalIgnoreCase))
    {
        Console.WriteLine("Deletion cancelled.");
        return 0;
    }

    var deleted = 0;
    foreach (var candidate in candidates)
    {
        try
        {
            candidate.FileItem.Delete();
            deleted++;
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            Console.Error.WriteLine($"Could not delete {candidate.FileItem.FullName}: {ex.Message}");
        }
    }

    Console.WriteLine($"Deleted {deleted} of {candidates.Count} proposed files.");
    return deleted == candidates.Count ? 0 : 1;
}

static string NormalizePath(string path) => OperatingSystem.IsWindows() ? path.Replace('\\', '/') : path;

static void WritePath(string path, bool highlight)
{
    if (!highlight || Console.IsOutputRedirected)
    {
        Console.WriteLine($"  {path}");
        return;
    }

    var previous = Console.ForegroundColor;
    try
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"  {path}");
    }
    finally
    {
        Console.ForegroundColor = previous;
    }
}
