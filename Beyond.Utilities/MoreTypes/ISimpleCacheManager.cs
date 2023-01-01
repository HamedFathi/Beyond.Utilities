// ReSharper disable UnusedMember.Global
namespace Beyond.Utilities.MoreTypes
{
    public interface ISimpleCacheManager<TKey, TValue> : IEnumerable where TKey : notnull
    {
        Dictionary<TKey, TValue?> CacheData { get; }

        TValue? this[TKey key] { get; set; }

        bool Contains(TKey key);

        int Count();

        TValue? Get(TKey key);

        TValue? GetOrSet(TKey key, Func<TValue> value);

        TValue? GetOrSet(TKey key, TValue? value);

        IEnumerable<TKey> Keys();

        bool Remove(TKey key);

        void RemoveAll();

        void Set(TKey key, TValue? value);

        TValue? TryGet(TKey key, out bool hasKey);

        bool TryRemove(TKey key, out TValue? value);

        bool TrySet(TKey key, TValue? value);

        IEnumerable<TValue?> Values();
    }
}