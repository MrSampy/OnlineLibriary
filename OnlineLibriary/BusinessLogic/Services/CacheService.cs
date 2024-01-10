using BusinessLogic.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace BusinessLogic.Services
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;

        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public T Get<T>(string key)
        {
            return _memoryCache.Get<T>(key);
        }

        public void Set<T>(string key, T value, TimeSpan duration)
        {
            _memoryCache.Set(key, value, duration);
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }

        public void Reset()
        {
            if (_memoryCache is MemoryCache cache)
            {
                cache.Clear();
            }
        }
    }

}
