using System;
using System.Threading.Tasks;
using Core.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace Infrastructure.Services
{
    public class ResponseCacheService : IResponseCacheService
    {
        private readonly IDatabase database;
        public ResponseCacheService(IConnectionMultiplexer redis)
        {
            database = redis.GetDatabase();
        }

        public async Task CacheReponseAsync(string cacheKey, object response, TimeSpan timeToLive)
        {
            // Validate data can store in redis
            if (response == null)
            {
                return;
            }

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var serializeReponse = JsonSerializer.Serialize(response, options);

            await database.StringSetAsync(cacheKey, serializeReponse, timeToLive);
        }

        public async Task<string> GetCacheResponseAsync(string cacheKey)
        {
            var result = await database.StringGetAsync(cacheKey);

            if (string.IsNullOrEmpty(result))
            {
                return null;
            }

            return result;
        }
    }
}