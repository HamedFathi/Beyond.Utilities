// ReSharper disable UnusedMember.Global
namespace Beyond.Utilities.MoreTypes;

public class TreeObject<T>
{
    public T Id { get; set; } = default!;
    public T? ParentId { get; set; }
    public string? Name { get; set; }
    public IList<TreeObject<T>> Children { get; set; }
    public TreeObject() => Children = new List<TreeObject<T>>();
}