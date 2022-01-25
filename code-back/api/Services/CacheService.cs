using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dockerapi.Services
{
    public class CacheService:IDisposable
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger<CacheService> _logger;
        public CacheService(ILogger<CacheService> logger, IDistributedCache cache)
        {
            _logger = logger;
            _cache = cache;
        }
                      

        public async Task<T> TryGet<T>(string key, Func<T> function)
        {
            return await TryGet(key, new TimeSpan(0, 1, 0), function);
        }
          
        
        public async Task<T> TryGet<T>(string key, TimeSpan cacheOut, Func<T> function)
        {
         
            // Get data from cache
            var cachedData = await _cache.GetAsync(key);
            if (cachedData != null)
            {
                // If data found in cache, encode and deserialize cached data
                var cachedDataString = Encoding.UTF8.GetString(cachedData);
                return JsonConvert.DeserializeObject<T>(cachedDataString);
            }
            else
            {
                // If not found, then fetch data from source
                var sourceData = function();

                // serialize data
                var cachedDataString = JsonConvert.SerializeObject(sourceData);
                var newDataToCache = Encoding.UTF8.GetBytes(cachedDataString);

                // set cache options 
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(cacheOut);
                    //.SetSlidingExpiration(cacheOut);

                // Add data in cache
                await _cache.SetAsync(key, newDataToCache, options);
                return sourceData;
            }

        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
        