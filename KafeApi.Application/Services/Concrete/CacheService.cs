using KafeApi.Application.Services.Abstract;
using Microsoft.Extensions.Caching.Memory;

namespace KafeApi.Application.Services.Concrete
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;

        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        //public T Get<T>(string key)
        //{
        //    return _memoryCache.Get<T>(key);
        //}

        public void Set<T>(string key, T value ,MemoryCacheEntryOptions options)
        {
            _memoryCache.Set(key, value , options);
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }

        public bool TryGetValue<T>(string key, out T value)
        {
            return _memoryCache.TryGetValue(key, out value);
        }
    }
}
