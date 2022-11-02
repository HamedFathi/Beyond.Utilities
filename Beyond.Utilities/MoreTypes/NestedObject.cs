// ReSharper disable UnusedMember.Global
namespace Beyond.Utilities.MoreTypes;

public class NestedObject<T>
{
    public NestedObject() => Children = new List<NestedObject<T>>();

    public IList<NestedObject<T>> Children { get; set; }
    public T? Id { get; set; }
}