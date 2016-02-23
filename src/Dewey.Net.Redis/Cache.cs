using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Dewey.Net.Redis
{
    public static class Cache
    {
        public static string ConnectionString { get; set; } = null;

        private static IDatabase _cache => lazyConnection.Value.GetDatabase();
        private static IServer _server => lazyConnection.Value.GetServer(ConnectionString);

        public static int SlidingExpirationMinutes = 30;

        private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            if (ConnectionString == null) {
                throw new ArgumentNullException("A connection string must be set.");
            }

            return ConnectionMultiplexer.Connect(ConnectionString);
        });

        private static TimeSpan TimeToExpire => new TimeSpan(0, SlidingExpirationMinutes, 0);

        private static string GetCacheKey<T>(string key)
        {
            string typeName = typeof(T).Name;

            if (key.Contains(typeName)) {
                return key;
            }

            return string.Format($"{typeName}-{key}");
        }

        public static void Set<T>(string key, T value)
        {
            key = GetCacheKey<T>(key);

            _cache.StringSet(key, JsonConvert.SerializeObject(value), TimeToExpire);
        }

        public static void Remove<T>(string key)
        {
            key = GetCacheKey<T>(key);

            _cache.KeyDelete(key);
        }

        public static void Clear()
        {
            lazyConnection.Value.GetServer(ConnectionString).FlushDatabase();
        }

        public static bool Contains<T>(string key)
        {
            key = GetCacheKey<T>(key);

            return _cache.KeyExists(key);
        }

        public static bool Get<T>(string key, out T value)
        {
            try {
                key = GetCacheKey<T>(key);

                if (!Contains<T>(key)) {
                    value = default(T);

                    return false;
                }

                value = JsonConvert.DeserializeObject<T>(_cache.StringGet(key));
                _cache.KeyExpire(key, TimeToExpire);

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

        public async static Task Flush(string pattern)
        {
            var keys = _server.Keys(pattern: $"*{pattern}*").ToArray();

            await _cache.KeyDeleteAsync(keys, CommandFlags.FireAndForget);
        }
    }
}
