// ReSharper disable UnusedMember.Global

namespace Beyond.Utilities.MoreTypes;

public class LruCache<TKey, TValue> where TKey : notnull
{
    private readonly Dictionary<TKey, LinkedListNode<LruCacheItem>> _cacheMap = new();

    private readonly Action<TValue?>? _dispose;

    private readonly LinkedList<LruCacheItem> _lruList = new();

    public LruCache(int capacity, Action<TValue?>? dispose = null)
    {
        Capacity = capacity;
        this._dispose = dispose;
    }

    public int Capacity { get; }

    public TValue? Get(TKey key)
    {
        lock (_cacheMap)
        {
            var status = TryGetValue(key, out var value);
            return status ? value : default;
        }
    }

    public TValue? Get(TKey key, Func<TValue> valueGenerator)
    {
        lock (_cacheMap)
        {
            TValue? value;
            if (_cacheMap.TryGetValue(key, out var node))
            {
                value = node.Value.Value;
                _lruList.Remove(node);
                _lruList.AddLast(node);
            }
            else
            {
                value = valueGenerator();
                if (_cacheMap.Count >= Capacity)
                {
                    RemoveFirst();
                }

                var cacheItem = new LruCacheItem(key, value);
                node = new LinkedListNode<LruCacheItem>(cacheItem);
                _lruList.AddLast(node);
                _cacheMap.Add(key, node);
            }

            return value;
        }
    }

    public void Set(TKey key, TValue value)
    {
        lock (_cacheMap)
        {
            if (_cacheMap.Count >= Capacity)
            {
                RemoveFirst();
            }

            var cacheItem = new LruCacheItem(key, value);
            var node =
                new LinkedListNode<LruCacheItem>(cacheItem);
            _lruList.AddLast(node);
            _cacheMap.Add(key, node);
        }
    }

    public bool TryGetValue(TKey key, out TValue? value)
    {
        lock (_cacheMap)
        {
            if (_cacheMap.TryGetValue(key, out var node))
            {
                value = node.Value.Value;
                _lruList.Remove(node);
                _lruList.AddLast(node);
                return true;
            }

            value = default;
            return false;
        }
    }

    private void RemoveFirst()
    {
        var node = _lruList.First;
        _lruList.RemoveFirst();
        if (node != null)
        {
            _cacheMap.Remove(node.Value.Key);
            _dispose?.Invoke(node.Value.Value);
        }
    }

    private class LruCacheItem
    {
        public LruCacheItem(TKey k, TValue? v)
        {
            Key = k;
            Value = v;
        }

        public TKey Key { get; }

        public TValue? Value { get; }
    }
}