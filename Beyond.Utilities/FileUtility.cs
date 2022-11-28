// ReSharper disable UnusedMember.Global
namespace Beyond.Utilities;

public static class FileUtility
{
    public static void Copy(string destination, string target)
    {
        var directory = Path.GetDirectoryName(target);
        Directory.CreateDirectory(directory ?? throw new InvalidOperationException());
        File.Copy(destination, target);
    }
}