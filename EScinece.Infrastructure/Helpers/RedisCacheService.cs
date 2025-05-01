using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace EScinece.Infrastructure.Helpers;

public class RedisCacheService
{
    private readonly IDistributedCache _cache;

    public RedisCacheService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiry ?? TimeSpan.FromMinutes(5)
        };
        
        await _cache.SetStringAsync(key, JsonSerializer.Serialize(value), options);
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var cachedData = await _cache.GetStringAsync(key);
        return cachedData == null ? default : JsonSerializer.Deserialize<T>(cachedData);
    }

    public async Task RemoveAsync(string key)
    {
        await _cache.RemoveAsync(key);
    }
}