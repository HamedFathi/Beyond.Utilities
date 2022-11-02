// ReSharper disable CheckNamespace
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace Beyond.Utilities;

public static class AssemblyUtility
{
    public static IEnumerable<Assembly>? GetEntryAssemblyWithReferences()
    {
        var listOfAssemblies = new List<Assembly>();
        var mainAsm = Assembly.GetEntryAssembly();

        if (mainAsm == null) return null;

        listOfAssemblies.Add(mainAsm);
        listOfAssemblies.AddRange(mainAsm.GetReferencedAssemblies().Select(Assembly.Load));
        return listOfAssemblies;
    }
}