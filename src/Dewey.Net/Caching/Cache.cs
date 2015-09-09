using System;
using System.Runtime.Caching;

namespace Dewey.Net.Caching
{
    public static class Cache<T>
    {
        private static CacheItemPolicy _policy => new CacheItemPolicy()
        {
            SlidingExpiration = TimeSpan.FromMinutes(30)
        };

        private static MemoryCache _cache = new MemoryCache("MemoryCache");

        public static void SetExpiration(int expiration)
        {
            _policy.SlidingExpiration = TimeSpan.FromMinutes(expiration);
        }

        private static string GetCacheKey(string key)
        {
            string typeName = typeof(T).Name;

            if (key.Contains(typeName)) {
                return key;
            }

            return string.Format("{0}-{1}", typeName, key);
        }

        public static void Set(string key, T value)
        {
            key = GetCacheKey(key);

            _cache.Set(key, value, _policy);
        }

        public static void Remove(string key)
        {
            key = GetCacheKey(key);

            _cache.Remove(key);
        }

        public static void Clear()
        {
            _cache.Dispose();

            _cache = new MemoryCache("MemoryCache");
        }

        public static bool Contains(string key)
        {
            key = GetCacheKey(key);

            return _cache.Contains(key);
        }

        public static bool Get(string key, out T value)
        {
            try {
                key = GetCacheKey(key);

                if (!Contains(key)) {
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
    }
}