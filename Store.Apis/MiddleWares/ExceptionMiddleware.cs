using Store.Apis.Errors;
using System.Text.Json;

namespace Store.Apis.MiddleWares
{
    public class ExceptionMiddleware
    {
        private readonly ILogger<ExceptionMiddleware> logger;
        private readonly RequestDelegate next;
        private readonly IHostEnvironment env;

        public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger, RequestDelegate next, IHostEnvironment env)
        {
            this.logger = logger;
            this.next = next;
            this.env = env;
        }


        
        public async Task InvokeAsync(HttpContext context)//Context to get request and Response
        {
            try
            {
                //To Invoke the next middleware.
                await next.Invoke(context);
            }
            catch (Exception ex)
            {

                logger.LogError(ex, ex.Message);

                //Content Type of the Error
                context.Response.ContentType = "application/json";
                //Status Code
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                var res = env.IsDevelopment() ?
                    new ApiExceptionResponse(StatusCodes.Status500InternalServerError,ex.Message,ex.StackTrace.ToString())
                    : new ApiExceptionResponse(StatusCodes.Status500InternalServerError);

                //To Convert Any Type into Json File
                var json = JsonSerializer.Serialize(res);
                //Return the Error to the Body of the Response
                context.Response.WriteAsync(json);

                //Will complete till the endpoint if the endpoint sends an exception
                //it will Log the exception and send it to the response (more readable)

            }



        }
    }
}
