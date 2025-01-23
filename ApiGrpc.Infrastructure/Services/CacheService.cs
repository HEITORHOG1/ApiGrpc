using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace ApiGrpc.Infrastructure.Services
{
    public class CacheService
    {
        private readonly IDistributedCache _cache;

        public CacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<T?> GetAsync<T>(string cacheKey)
        {
            var cachedData = await _cache.GetStringAsync(cacheKey);
            return cachedData == null ? default : JsonSerializer.Deserialize<T>(cachedData);
        }

        public async Task SetAsync<T>(string cacheKey, T data, DistributedCacheEntryOptions options)
        {
            var serializedData = JsonSerializer.Serialize(data);
            await _cache.SetStringAsync(cacheKey, serializedData, options);
        }
    }
}