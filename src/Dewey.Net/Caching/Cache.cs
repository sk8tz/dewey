using System;
using System.Runtime.Caching;

namespace Dewey.Net.Caching
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

        public static void SetExpiration(int expiration)
        {
            _policy.SlidingExpiration = TimeSpan.FromMinutes(expiration);
        }

        private static string GetCacheKey<T>(string key)
        {
            string typeName = typeof(T).Name;

            if (key.Contains(typeName)) {
                return key;
            }

            return string.Format("{0}-{1}", typeName, key);
        }
        
        public static void Set<T>(string key, T value)
        {
            key = GetCacheKey<T>(key);

            _cache.Set(key, value, _policy);
        }

        public static void Remove<T>(string key)
        {
            key = GetCacheKey<T>(key);

            _cache.Remove(key);
        }

        public static void Clear()
        {
            foreach (var element in _cache) {
                _cache.Remove(element.Key);
            }
        }

        public static bool Contains<T>(string key)
        {
            key = GetCacheKey<T>(key);

            return _cache.Contains(key);
        }

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
    }
}