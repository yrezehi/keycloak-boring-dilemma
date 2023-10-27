﻿using Microsoft.Extensions.Caching.Memory;

namespace ThirdPartyAuthentication.Caches
{
    public class CacheProvider { 
        private readonly IMemoryCache Cache;

        public CacheProvider(IMemoryCache memoryCache) =>
            Cache = memoryCache;

        public bool Contains(string key) =>
            Cache.TryGetValue(key, out _);

        public void Remove(string name, string key) =>
            Cache.Remove(key);

        public void Set<T>(string name, string key, T value) =>
            Cache.Set(key, value);

        public T? Get<T>(string name, string key) =>
            Cache.TryGetValue(key, out T? value) ? value : default;
    }
}
