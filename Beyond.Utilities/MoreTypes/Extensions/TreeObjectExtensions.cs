// ReSharper disable CheckNamespace
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable CheckNamespace

namespace Beyond.Utilities.MoreTypes;

public static class TreeObjectExtensions
{
    public static IEnumerable<ITreeObject<T>> ToFlatten<T>(this ITreeObject<T> treeObject, bool addRootObject = true)
    {
        if (addRootObject)
            yield return treeObject;

        foreach (var item in treeObject.Children)
        {
            foreach (var data in ToFlatten(item))
            {
                yield return data;
            }
        }
    }

    public static void Walk<T>(this ITreeObject<T> treeObject, Action<ITreeObject<T>> action, bool checkRootObject = true)
    {
        if (checkRootObject)
            action(treeObject);

        foreach (var item in treeObject.Children)
        {
            foreach (var data in ToFlatten(item))
            {
                action(data);
            }
        }
    }

    public static IEnumerable<ITreeObject<T>> ToHierarchy<T>(this IEnumerable<ITreeObject<T>> flattenTreeObject) where T : notnull
    {
        // if ParentId is null => this is root.
        var lookup = new Dictionary<T, ITreeObject<T>>();
        var nested = new List<ITreeObject<T>>();

        foreach (var item in flattenTreeObject)
        {
            if (item.ParentId != null && lookup.ContainsKey(item.ParentId))
            {
                lookup[item.ParentId].Children.Add(item);
            }
            else
            {
                nested.Add(item);
            }
            lookup.Add(item.Id, item);
        }
        return nested;
    }
}