// ReSharper disable CheckNamespace
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace Beyond.Utilities;

public static class DirectoryUtility
{
    public static void Copy(string sourceDir, string targetDir, bool overwrite)
    {
        Directory.CreateDirectory(targetDir);

        foreach (var file in Directory.GetFiles(sourceDir))
            File.Copy(file, Path.Combine(targetDir, Path.GetFileName(file)), overwrite);

        foreach (var directory in Directory.GetDirectories(sourceDir))
            Copy(directory, Path.Combine(targetDir, Path.GetFileName(directory)), overwrite);
    }

    public static void CreateDirectoryRecursively(string path, char separator = '\\')
    {
        var pathParts = path.Split(separator);
        for (var i = 0; i < pathParts.Length; i++)
        {
            if (i > 0)
                pathParts[i] = Path.Combine(pathParts[i - 1], pathParts[i]);

            if (!Directory.Exists(pathParts[i]))
                Directory.CreateDirectory(pathParts[i]);
        }
    }

    public static string CreateTempDirectory()
    {
        var tempDirectory = GetTempDirectory();
        if (!Directory.Exists(tempDirectory)) Directory.CreateDirectory(tempDirectory);

        return tempDirectory;
    }

    public static void CreateTempDirectory(Action<string> action, bool autoDelete = true)
    {
        var tempDirectory = GetTempDirectory();
        if (!Directory.Exists(tempDirectory)) Directory.CreateDirectory(tempDirectory);
        action(tempDirectory);
        if (autoDelete) Directory.Delete(tempDirectory, true);
    }

    public static void DeleteReadOnlyDirectory(string directoryPath)
    {
        foreach (var subDirectory in Directory.EnumerateDirectories(directoryPath))
            DeleteReadOnlyDirectory(subDirectory);
        foreach (var fileName in Directory.EnumerateFiles(directoryPath))
        {
            var fileInfo = new FileInfo(fileName)
            {
                Attributes = FileAttributes.Normal
            };
            fileInfo.Delete();
        }

        Directory.Delete(directoryPath);
    }

    public static void Empty(this DirectoryInfo directory)
    {
        try
        {
            foreach (var file in directory.GetFiles()) file.Delete();
            foreach (var subDirectory in directory.GetDirectories()) subDirectory.Delete(true);
        }
        catch
        {
            // ignored
        }
    }

    public static string? GetDirectoryPath(string filePath)
    {
        return Path.GetDirectoryName(filePath);
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public static string? GetParentDirectoryPath(string folderPath, int levels)
    {
        var result = folderPath;
        for (var i = 0; i < levels; i++)
            if (result != null && Directory.GetParent(result) != null)
                result = Directory.GetParent(result)?.FullName;
            else
                return result;
        return result;
    }

    public static string? GetParentDirectoryPath(string folderPath)
    {
        return GetParentDirectoryPath(folderPath, 1);
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public static string GetTempDirectory()
    {
        return Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
    }

    public static bool RenameDirectory(string directoryPath, string newName)
    {
        var path = Path.GetDirectoryName(directoryPath);
        if (path == null)
            return false;
        try
        {
            Directory.Move(directoryPath, Path.Combine(path, newName));
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static void SafeDeleteDirectory(string path, bool recursive = false)
    {
        if (!Directory.Exists(path)) return;

        var searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
        var files = Directory
              .EnumerateFileSystemEntries(path, "*", searchOption);
        foreach (var file in files)
        {
            File.SetAttributes(file, FileAttributes.Normal);
        }
        Directory.Delete(path, recursive);
    }
}