#if !DNXCORE50
using System;
using System.Runtime.Caching;

namespace Dewey.Caching
{
    public static class Cache
    {
        private static readonly MemoryCache _cache = new MemoryCache("MemoryCache");

        public static MemoryCache Instance => _cache;

        public static int SlidingExpirationMinutes = 30;

        private static CacheItemPolicy _policy => new CacheItemPolicy()
        {
            SlidingExpiration = TimeSpan.FromMinutes(SlidingExpirationMinutes)
        };

        public static void SetExpiration(int expiration) => _policy.SlidingExpiration = TimeSpan.FromMinutes(expiration);

        private static string GetCacheKey<T>(string key) => (key.Contains(typeof(T).Name)) ? key : string.Format("{0}-{1}", typeof(T).Name, key);
        
        public static void Set<T>(string key, T value) => _cache.Set(GetCacheKey<T>(key), value, _policy);

        public static void Remove<T>(string key) => _cache.Remove(GetCacheKey<T>(key));

        public static bool Contains<T>(string key) => _cache.Contains(GetCacheKey<T>(key));

        public static bool Get<T>(string key, out T value)
        {
            try {
                key = GetCacheKey<T>(key);

                if (!Contains<T>(key)) {
                    value = default(T);

                    return false;
                }

                value = (T)_cache.Get(key);

                if (value != null) {
                    return true;
                }

                value = default(T);

                return false;
            } catch {
                value = default(T);

                return false;
            }
        }
        
        public static void Clear()
        {
            foreach (var element in _cache) {
                _cache.Remove(element.Key);
            }
        }
    }
}
#endif