// ReSharper disable UnusedMember.Global

namespace Beyond.Utilities.MoreTypes;

public sealed class AdHocEquatable<T> : IEquatable<T>
{
    private readonly Func<T, bool> equals;
    public AdHocEquatable(Func<T, bool> eq)
    {
        equals = eq;
    }
    public bool Equals(T? other)
    {
        if (other is null)
            return false;
        return equals(other);
    }
}
