// ReSharper disable UnusedMember.Global

namespace Beyond.Utilities.MoreTypes;

public class LruCache<TKey, TValue> where TKey : notnull
{
    private readonly Dictionary<TKey, LinkedListNode<LruCacheItem>> cacheMap = new();

    private readonly Action<TValue?>? dispose;

    private readonly LinkedList<LruCacheItem> lruList = new();

    public LruCache(int capacity, Action<TValue?>? dispose = null)
    {
        Capacity = capacity;
        this.dispose = dispose;
    }

    public int Capacity { get; }

    public TValue? Get(TKey key)
    {
        lock (cacheMap)
        {
            var status = TryGetValue(key, out var value);
            return status ? value : default;
        }
    }

    public TValue? Get(TKey key, Func<TValue> valueGenerator)
    {
        lock (cacheMap)
        {
            TValue? value;
            if (cacheMap.TryGetValue(key, out var node))
            {
                value = node.Value.Value;
                lruList.Remove(node);
                lruList.AddLast(node);
            }
            else
            {
                value = valueGenerator();
                if (cacheMap.Count >= Capacity)
                {
                    RemoveFirst();
                }

                var cacheItem = new LruCacheItem(key, value);
                node = new LinkedListNode<LruCacheItem>(cacheItem);
                lruList.AddLast(node);
                cacheMap.Add(key, node);
            }

            return value;
        }
    }

    public void Set(TKey key, TValue value)
    {
        lock (cacheMap)
        {
            if (cacheMap.Count >= Capacity)
            {
                RemoveFirst();
            }

            var cacheItem = new LruCacheItem(key, value);
            var node =
                new LinkedListNode<LruCacheItem>(cacheItem);
            lruList.AddLast(node);
            cacheMap.Add(key, node);
        }
    }

    public bool TryGetValue(TKey key, out TValue? value)
    {
        lock (cacheMap)
        {
            if (cacheMap.TryGetValue(key, out var node))
            {
                value = node.Value.Value;
                lruList.Remove(node);
                lruList.AddLast(node);
                return true;
            }

            value = default;
            return false;
        }
    }

    private void RemoveFirst()
    {
        var node = lruList.First;
        lruList.RemoveFirst();
        if (node != null)
        {
            cacheMap.Remove(node.Value.Key);
            dispose?.Invoke(node.Value.Value);
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