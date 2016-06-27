﻿using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Dewey.Redis
{
    public static class Cache
    {
        private static IDatabase _cache => lazyConnection.Value.GetDatabase();

        public static int SlidingExpirationMinutes = 30;

        public static ConfigurationOptions ConfigurationOptions { get; set; }

        private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() => {
            if (ConfigurationOptions == null) {
                throw new ArgumentNullException("ConfigurationOptions must be set.");
            }

            return ConnectionMultiplexer.Connect(ConfigurationOptions);
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
            foreach (var endpoint in lazyConnection.Value.GetEndPoints()) {
                lazyConnection.Value.GetServer(endpoint).FlushDatabase();
            }
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
            if (!ConfigurationOptions.AllowAdmin) {
                throw new Exception("Flushing by pattern requires Admin mode.");
            }

            foreach (var endpoint in lazyConnection.Value.GetEndPoints()) {
                var keys = lazyConnection.Value.GetServer(endpoint).Keys(pattern: pattern).ToArray();

                await lazyConnection.Value.GetDatabase().KeyDeleteAsync(keys);
            }
        }
    }
}