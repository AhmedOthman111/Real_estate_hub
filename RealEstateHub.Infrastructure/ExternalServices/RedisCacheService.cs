using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json.Serialization;
using RealEstateHub.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RealEstateHub.Infrastructure.ExternalServices
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _cache;
        private readonly JsonSerializerOptions _serializerOptions;

        public RedisCacheService(IDistributedCache cache)
        {
            _cache = cache;
            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };
        }

        public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            var cachedata = await _cache.GetAsync(key, cancellationToken);
            if (cachedata is null) return default;

            return JsonSerializer.Deserialize<T>(cachedata, _serializerOptions);
        }

        public async Task SetAsync<T>( string key, T value, TimeSpan? absoluteExpireTime = null, TimeSpan? slidingExpireTime = null, CancellationToken cancellationToken = default)
        {
            var options = new DistributedCacheEntryOptions();

            if (absoluteExpireTime != null)
                options.AbsoluteExpirationRelativeToNow = absoluteExpireTime;

            if (slidingExpireTime != null)
                options.SlidingExpiration = slidingExpireTime;

            var serializedData = JsonSerializer.Serialize(value, _serializerOptions);

            await _cache.SetStringAsync(key, serializedData, options, cancellationToken);
        }

        public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            await _cache.RemoveAsync(key, cancellationToken);
        }



    }
}
