// ReSharper disable UnusedMember.Global
namespace Beyond.Utilities.MoreTypes
{
    public class SimpleCacheManager<TKey, TValue> : ISimpleCacheManager<TKey, TValue> where TKey : notnull
    {
        private readonly ConcurrentDictionary<TKey, TValue?> _cache = new();

        public Dictionary<TKey, TValue?> CacheData => _cache.ToDictionary(entry => entry.Key, entry => entry.Value);

        public TValue? this[TKey key]
        {
            get => _cache[key];
            set => _cache[key] = value;
        }

        public bool Contains(TKey key) => _cache.ContainsKey(key);

        public int Count() => _cache.Count;

        public TValue? Get(TKey key) => !_cache.ContainsKey(key) ? default : _cache[key];

        public IEnumerator GetEnumerator()
        {
            return _cache.GetEnumerator();
        }

        public TValue? GetOrSet(TKey key, TValue? value)
        {
            if (_cache.ContainsKey(key))
                return Get(key);
            Set(key, value);
            return value;
        }

        public TValue? GetOrSet(TKey key, Func<TValue> value) => GetOrSet(key, value());

        public IEnumerable<TKey> Keys() => _cache.Keys;

        public bool Remove(TKey key) => _cache.TryRemove(key, out _);

        public void RemoveAll()
        {
            _cache.Clear();
        }

        public void Set(TKey key, TValue? value) => _cache.AddOrUpdate(key, value, (_, _) => value);

        public TValue? TryGet(TKey key, out bool hasKey)
        {
            if (_cache.ContainsKey(key))
            {
                hasKey = true;
                return _cache[key];
            }

            hasKey = false;
            return default;
        }

        public bool TryRemove(TKey key, out TValue? value) => _cache.TryRemove(key, out value);

        public bool TrySet(TKey key, TValue? value)
        {
            if (_cache.ContainsKey(key))
            {
                Set(key, value);
                return true;
            }
            return false;
        }

        public IEnumerable<TValue?> Values() => _cache.Values;
    }
}
