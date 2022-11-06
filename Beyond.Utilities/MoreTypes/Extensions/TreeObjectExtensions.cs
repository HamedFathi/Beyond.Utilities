// ReSharper disable CheckNamespace
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable CheckNamespace

namespace Beyond.Utilities.MoreTypes;

public static class TreeObjectExtensions
{
    public static IEnumerable<TreeObject<T>> ToFlatten<T>(this TreeObject<T> treeObject, bool addRootObject = true)
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

    public static void Walk<T>(this TreeObject<T> treeObject, Action<TreeObject<T>> action, bool checkRootObject = true)
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

    public static IEnumerable<TreeObject<T>> ToHierarchy<T>(this IEnumerable<TreeObject<T>> flattenTreeObject) where T : notnull
    {
        // if ParentId is null => this is root.
        var lookup = new Dictionary<T, TreeObject<T>>();
        var nested = new List<TreeObject<T>>();

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