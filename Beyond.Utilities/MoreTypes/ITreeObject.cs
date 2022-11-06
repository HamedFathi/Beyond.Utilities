// ReSharper disable UnusedMember.Global

namespace Beyond.Utilities.MoreTypes;

public interface ITreeObject<T>
{
    public T Id { get; set; }
    public T? ParentId { get; set; }
    public string? Name { get; set; }
    public IList<ITreeObject<T>> Children { get; set; }
}