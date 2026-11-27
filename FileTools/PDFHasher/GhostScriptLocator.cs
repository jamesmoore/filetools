using Microsoft.Win32;
using System;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Runtime.Versioning;

namespace FileTools.PDFHasher
{
    public class GhostScriptLocator(IFileSystem fileSystem)
    {
        public string GetLocation()
        {
            if (OperatingSystem.IsWindows())
                return FindOnWindows();

            return OperatingSystem.IsLinux() ? FindOnPath("gs") : null;
        }

        [SupportedOSPlatform("windows")]
        private string FindOnWindows()
        {
            string[] executableNames = ["gswin64c.exe", "gswin32c.exe"];
            var onPath = FindOnPath(executableNames);
            if (onPath != null)
                return onPath;

            foreach (var hive in new[] { RegistryHive.CurrentUser, RegistryHive.LocalMachine })
            {
                foreach (var view in new[] { RegistryView.Registry64, RegistryView.Registry32 })
                {
                    using var root = RegistryKey.OpenBaseKey(hive, view);
                    using var ghostscript = root.OpenSubKey(@"SOFTWARE\GPL Ghostscript");
                    if (ghostscript == null)
                        continue;

                    foreach (var version in ghostscript.GetSubKeyNames().OrderByDescending(ParseVersion))
                    {
                        using var installation = ghostscript.OpenSubKey(version);
                        if (installation?.GetValue("GS_DLL") is not string dllPath ||
                            !Path.IsPathFullyQualified(dllPath))
                            continue;

                        var executable = FindInDirectory(Path.GetDirectoryName(dllPath), executableNames);
                        if (executable != null)
                            return executable;
                    }
                }
            }

            foreach (var folder in new[] { Environment.SpecialFolder.ProgramFiles, Environment.SpecialFolder.ProgramFilesX86 })
            {
                var programFiles = Environment.GetFolderPath(folder);
                if (string.IsNullOrEmpty(programFiles))
                    continue;

                var installRoot = Path.Combine(programFiles, "gs");
                if (!fileSystem.Directory.Exists(installRoot))
                    continue;

                foreach (var directory in fileSystem.Directory.EnumerateDirectories(installRoot, "gs*")
                    .OrderByDescending(path => ParseVersion(Path.GetFileName(path).Substring(2))))
                {
                    var executable = FindInDirectory(Path.Combine(directory, "bin"), executableNames);
                    if (executable != null)
                        return executable;
                }
            }

            return null;
        }

        private string FindOnPath(params string[] executableNames)
        {
            var path = Environment.GetEnvironmentVariable("PATH") ?? string.Empty;
            foreach (var entry in path.Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries))
            {
                var directory = OperatingSystem.IsWindows() ? entry.Trim('"') : entry;
                if (!Path.IsPathFullyQualified(directory))
                    continue;

                var executable = FindInDirectory(directory, executableNames);
                if (executable != null)
                    return executable;
            }

            return null;
        }

        private string FindInDirectory(string directory, string[] executableNames) =>
            executableNames.Select(name => Path.Combine(directory, name))
                .FirstOrDefault(path => fileSystem.File.Exists(path) &&
                    (!OperatingSystem.IsLinux() ||
                     (fileSystem.File.GetUnixFileMode(path) &
                      (UnixFileMode.UserExecute | UnixFileMode.GroupExecute | UnixFileMode.OtherExecute)) != 0));

        private static Version ParseVersion(string value) =>
            Version.TryParse(value, out var version) ? version : null;
    }
}
