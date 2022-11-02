// ReSharper disable CheckNamespace
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace Beyond.Utilities;

public static class DotNetUtility
{
    public static bool IsDebugMode()
    {
#if DEBUG
        return true;
#else
        return false;
#endif
    }
}