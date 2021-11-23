using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UrlShortener.Extractions
{
    public static class DistributedCacheExtraction
    {
        public static async Task SetRecordAsync<T>(this IDistributedCache cache,
           string recordId,
           T data,
           TimeSpan? absoluteExpireTime = null,
           TimeSpan? unuserExpireTime = null)
        {
            try
            {
                var options = new DistributedCacheEntryOptions();

                options.AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromMinutes(1);
                options.SlidingExpiration = unuserExpireTime;

                var jsonData = JsonConvert.SerializeObject(data);
                await cache.SetStringAsync(recordId, jsonData, options);
            }
            catch (Exception e) { }
        }

        public static async Task<T> GetRecordAsync<T>(this IDistributedCache cache, string recordId)
        {
            try
            {
                var jsonData = await cache.GetStringAsync(recordId);
                return (T)JsonConvert.DeserializeObject(jsonData, typeof(T));
            }
            catch(Exception e)
            {
                return default(T);
            }
        }
    }
}
