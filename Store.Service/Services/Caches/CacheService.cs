using StackExchange.Redis;
using Store.Core.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.Service.Services.Caches
{
    public class CacheService : ICacheServices
    {
        private readonly IDatabase database;

        public CacheService(IConnectionMultiplexer redis)
        {
            database = redis.GetDatabase();
        }


        public async Task<string> GetCacheAsync(string key)
        {
            var cacheResponse = await database.StringGetAsync(key);

            if (cacheResponse.IsNullOrEmpty) return null;

            return cacheResponse.ToString();
        }

        public async Task SetCacheAsync(string key, object value, TimeSpan expireTime)
        {
            if (value is null) return;


            var options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            var Response = JsonSerializer.Serialize(value,options);

            await database.StringSetAsync(key, Response, expireTime);


        }
    }
}
