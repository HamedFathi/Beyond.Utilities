namespace Beyond.Utilities.MoreTypes;

public class NullValueDictionary<TKey, TValue> : Dictionary<TKey, TValue>, INullValueDictionary<TKey, TValue> where TKey : notnull
{
    private readonly TValue? _defaultValue;
    public NullValueDictionary()
    {
        _defaultValue = default;
    }
    public NullValueDictionary(TValue? defaultValue)
    {
        _defaultValue = defaultValue;
    }
    
    public new TValue this[TKey key]
    {
        get => TryGetValue(key, out var val) ? val : _defaultValue!;
        set => base[key] = value;
    }
}