namespace Beyond.Utilities.MoreTypes;

public interface INullValueDictionary<TKey, TValue> : IDictionary<TKey, TValue> where TKey : notnull
{
    new TValue this[TKey key] { get; set; }
}