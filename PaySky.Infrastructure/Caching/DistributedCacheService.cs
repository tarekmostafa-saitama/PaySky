using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using PaySky.Application.Common.Caching;
using PaySky.Application.Common.Services;

namespace PaySky.Infrastructure.Caching;

#pragma warning disable CA2254
public class DistributedCacheService(IDistributedCache cache, ISerializerService serializer,
        ILogger<DistributedCacheService> logger)
    : ICacheService
{
    public T Get<T>(string key)
    {
        return Get(key) is { } data
            ? Deserialize<T>(data)
            : default;
    }

    public async Task<T> GetAsync<T>(string key, CancellationToken token = default)
    {
        return await GetAsync(key, token) is { } data
            ? Deserialize<T>(data)
            : default;
    }

    public void Refresh(string key)
    {
        try
        {
            cache.Refresh(key);
        }
        catch
        {
        }
    }

    public async Task RefreshAsync(string key, CancellationToken token = default)
    {
        try
        {
            await cache.RefreshAsync(key, token);
            logger.LogDebug(string.Format("Cache Refreshed : {0}", key));
        }
        catch
        {
        }
    }

    public void Remove(string key)
    {
        try
        {
            cache.Remove(key);
        }
        catch
        {
        }
    }

    public async Task RemoveAsync(string key, CancellationToken token = default)
    {
        try
        {
            await cache.RemoveAsync(key, token);
        }
        catch
        {
        }
    }

    public void Set<T>(string key, T value, TimeSpan? slidingExpiration = null)
    {
        Set(key, Serialize(value), slidingExpiration);
    }

    public Task SetAsync<T>(string key, T value, TimeSpan? slidingExpiration = null,
        CancellationToken cancellationToken = default)
    {
        return SetAsync(key, Serialize(value), slidingExpiration, cancellationToken);
    }

    private byte[] Get(string key)
    {
        ArgumentNullException.ThrowIfNull(key);

        try
        {
            return cache.Get(key);
        }
        catch
        {
            return null;
        }
    }

    private async Task<byte[]> GetAsync(string key, CancellationToken token = default)
    {
        try
        {
            return await cache.GetAsync(key, token);
        }
        catch
        {
            return null;
        }
    }

    private void Set(string key, byte[] value, TimeSpan? slidingExpiration = null)
    {
        try
        {
            cache.Set(key, value, GetOptions(slidingExpiration));
            logger.LogDebug($"Added to Cache : {key}");
        }
        catch
        {
        }
    }

    private async Task SetAsync(string key, byte[] value, TimeSpan? slidingExpiration = null,
        CancellationToken token = default)
    {
        try
        {
            await cache.SetAsync(key, value, GetOptions(slidingExpiration), token);
            logger.LogDebug($"Added to Cache : {key}");
        }
        catch
        {
        }
    }

    private byte[] Serialize<T>(T item)
    {
        return Encoding.Default.GetBytes(serializer.Serialize(item));
    }

    private T Deserialize<T>(byte[] cachedData)
    {
        return serializer.Deserialize<T>(Encoding.Default.GetString(cachedData));
    }

    private static DistributedCacheEntryOptions GetOptions(TimeSpan? slidingExpiration)
    {
        var options = new DistributedCacheEntryOptions();
        if (slidingExpiration.HasValue)
            options.SetSlidingExpiration(slidingExpiration.Value);
        else
            options.SetSlidingExpiration(TimeSpan.FromMinutes(10)); // Default expiration time of 10 minutes.

        return options;
    }
}