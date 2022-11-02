// ReSharper disable UnusedMember.Global
// ReSharper disable StringLiteralTypo

namespace Beyond.Utilities.MoreTypes;

public readonly struct Unit : IEquatable<Unit>, IComparable<Unit>, IComparable
{
    private static readonly Unit Default = new();

    public static Task<Unit> Task { get; } = System.Threading.Tasks.Task.FromResult(Default);
    public static ref readonly Unit Value => ref Default;

    public static implicit operator Unit(ValueTuple _) => default;

    public static implicit operator ValueTuple(Unit _) => default;

    [SuppressMessage("Roslynator", "RCS1163:Unused parameter.", Justification = "<Pending>")]
    [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
    public static bool operator !=(Unit first, Unit second) => false;

    [SuppressMessage("Roslynator", "RCS1163:Unused parameter.", Justification = "<Pending>")]
    [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
    public static Unit operator +(Unit first, Unit second) => Default;

    [SuppressMessage("Roslynator", "RCS1163:Unused parameter.", Justification = "<Pending>")]
    [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
    public static bool operator <(Unit first, Unit second) => false;

    [SuppressMessage("Roslynator", "RCS1163:Unused parameter.", Justification = "<Pending>")]
    [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
    public static bool operator <=(Unit first, Unit second) => true;

    [SuppressMessage("Roslynator", "RCS1163:Unused parameter.", Justification = "<Pending>")]
    [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
    public static bool operator ==(Unit first, Unit second) => true;

    [SuppressMessage("Roslynator", "RCS1163:Unused parameter.", Justification = "<Pending>")]
    [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
    public static bool operator >(Unit first, Unit second) => false;

    [SuppressMessage("Roslynator", "RCS1163:Unused parameter.", Justification = "<Pending>")]
    [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
    public static bool operator >=(Unit first, Unit second) => true;

    public int CompareTo(Unit other) => 0;

    int IComparable.CompareTo(object? obj) => 0;

    public bool Equals(Unit other) => true;

    public override bool Equals(object? obj) => obj is Unit;

    public override int GetHashCode() => 0;

    /// <summary>
    /// Provide an alternative value to unit
    /// </summary>
    /// <typeparam name="T">Alternative value type</typeparam>
    /// <param name="anything">Alternative value</param>
    /// <returns>Alternative value</returns>
    public T Return<T>(T anything) => anything;

    /// <summary>
    /// Provide an alternative value to unit
    /// </summary>
    /// <typeparam name="T">Alternative value type</typeparam>
    /// <param name="anything">Alternative value</param>
    /// <returns>Alternative value</returns>
    public T Return<T>(Func<T> anything) => anything();

    /// <summary>
    /// Provide an alternative value to unit
    /// </summary>
    /// <typeparam name="T">Alternative value type</typeparam>
    /// <param name="anything">Alternative value</param>
    /// <returns>Alternative value</returns>
    public T Return<T>(Expression<Func<T>> anything) => anything.Compile()();

    public override string ToString() => "()";
}