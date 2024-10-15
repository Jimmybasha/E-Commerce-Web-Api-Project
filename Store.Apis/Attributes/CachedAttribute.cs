using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Store.Core.Services.Contract;
using System.Text;

namespace Store.Apis.Attributes
{
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int expireTime;

        public CachedAttribute(int expireTime)
        { 
            this.expireTime = expireTime;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheServices>();

            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request); 

            var cacheResponse = await cacheService.GetCacheAsync(cacheKey);

            //if already Cached and found return the Response 
            if (!string.IsNullOrEmpty(cacheResponse)) 
            {

                //To Return the result
                var contentResult = new ContentResult()
                {
                    Content = cacheResponse,
                    ContentType = "application/json",
                    StatusCode= 200,

                };

                context.Result = contentResult;
                return;
            }
            //if not , use the Endpoint that u are already pointing to Using (next())
            var executedContext = await next();

            //If the result is ok and returned from endpoint return it in response variable
            if (executedContext.Result is OkObjectResult response)
            {
                //after that create a cache with the response i've just obtained from the endpoint
               await cacheService.SetCacheAsync(cacheKey, response.Value, TimeSpan.FromSeconds(expireTime));
            }


        }


        //Private method to generateTheCacheKey to use it before executing the Controller Methods [Cached] when sending request
        private string GenerateCacheKeyFromRequest(HttpRequest request)
        {

            var cacheKey = new StringBuilder();

            cacheKey.Append($"{request.Path}");

            foreach (var (key, value) in request.Query.OrderBy(K => K.Key))
            {
                cacheKey.Append($"|{key}-{value}");
            }

            return cacheKey.ToString(); 

        }


    }
}
