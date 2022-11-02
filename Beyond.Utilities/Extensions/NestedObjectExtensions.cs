// ReSharper disable CheckNamespace
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable CheckNamespace

namespace Beyond.Utilities.MoreTypes;

public static class NestedObjectExtensions
{
    public static IEnumerable<NestedObject<T>> Flatten<T>(this NestedObject<T> nestedObject, bool addRootObject = true)
    {
        if (addRootObject)
            yield return nestedObject;

        foreach (var item in nestedObject.Children)
        {
            foreach (var data in Flatten(item))
            {
                yield return data;
            }
        }
    }
}