// ReSharper disable CheckNamespace
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace Beyond.Utilities;

public static class CurrentFileUtility
{
    public static string Directory([CallerFilePath] string file = "") => System.IO.Path.GetDirectoryName(file)!;

    public static string Path([CallerFilePath] string file = "") => file;

    public static string Relative(string relative, [CallerFilePath] string file = "")
    {
        var directory = System.IO.Path.GetDirectoryName(file)!;
        return System.IO.Path.Combine(directory, relative);
    }
}