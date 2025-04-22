
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace SurvayBasket.services
{
    public class CacheService(IDistributedCache distributedCache) : ICacheService
    {
        private readonly IDistributedCache distributedCache = distributedCache;

        public async Task<T> GetAsync<T>(string cacheKey, CancellationToken cancellationToken) where T : class
        {
            var item = await distributedCache.GetStringAsync(cacheKey,cancellationToken);

            if (string.IsNullOrEmpty(item))
            {
               return null!;

            };
            return JsonSerializer.Deserialize<T>(item)!;
        }


        public async Task SetAsync<T>(string cacheKey, T Value, CancellationToken cancellationToken) where T : class
        {
            await distributedCache.SetStringAsync(cacheKey, JsonSerializer.Serialize(Value), cancellationToken);
        }


        public async Task RemoveAsync(string cacheKey, CancellationToken cancellationToken)
        {
            await  distributedCache.RemoveAsync(cacheKey , cancellationToken);
        }

    }
}
