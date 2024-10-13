using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Apis.Errors;

namespace Store.Apis.Controllers
{
    [Route("error/{code}")]
    [ApiController]
    //Ignore it bcz i use it only when something is off with the Api Endpoint
    [ApiExplorerSettings(IgnoreApi =true)]
    public class ErrorController : ControllerBase
    {

        public IActionResult Error(int code)
        {

            return NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound,"End Point isn't Found"));

        }



    }
}
