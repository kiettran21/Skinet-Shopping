using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace API.Helpers
{
    /// <summary>
    ///    Attribute Class Cached which get data from Redis cache.
    ///    Like Middleware, it runs firstly and move controllers if not data on redis.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int timeToLiveSeconds;

        public CachedAttribute(int timeToLiveSeconds)
        {
            this.timeToLiveSeconds = timeToLiveSeconds;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // 1. Get Service CacheResponse
            var cache = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();

            // 2. Create or Build the key cache
            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);

            // 3. Check key cache is exsists
            var cachedResponse = await cache.GetCacheResponseAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedResponse))
            {
                context.Result = new ContentResult
                {
                    Content = cachedResponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };

                return;
            }

            // 4. Else Create new cache
            var executedContext = await next(); // Move controller

            if (executedContext.Result is OkObjectResult okObjectResult)
            {
                await cache.CacheReponseAsync(cacheKey, okObjectResult.Value, TimeSpan.FromSeconds(timeToLiveSeconds));
            }
        }

        private string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            StringBuilder keyBuilder = new();

            keyBuilder.Append(request.Path);

            // Query string from url
            foreach (var (key, value) in request.Query.OrderBy(q => q.Key))
            {
                keyBuilder.Append($"|{key}-{value}");
            }

            return keyBuilder.ToString();
        }
    }
}