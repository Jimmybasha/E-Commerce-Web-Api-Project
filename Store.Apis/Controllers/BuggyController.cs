using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.Repository.Data.Contexts;

namespace Store.Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuggyController : ControllerBase
    {
        private readonly StoreDbContext context;

        public BuggyController(StoreDbContext context)
        {
            this.context = context;
        }


        [HttpGet("notfound")]
        public async Task<IActionResult> GetNotFoundError()
        {
            var product = await context.Products.FirstOrDefaultAsync(P=>P.Id==100);

            if (product == null) return NotFound(new {Message="Product with id :100 is not found", StatusCode = StatusCodes.Status404NotFound });

            return Ok(product);

        }
        
        
        [HttpGet("servererror")]
        public async Task<IActionResult> GetServerError()
        {
            var product = await context.Products.FirstOrDefaultAsync(P=>P.Id==100);

            var errorProduct = product.ToString();

            return Ok(errorProduct);

        }
        
        
        [HttpGet("badrequest")]
        public async Task<IActionResult> GetBadRequestError()
        {
         
            return BadRequest();

        }



        //Validation Error for examples id=ahmed
        [HttpGet("badrequest/{id}")]
        public async Task<IActionResult> GetBadRequestError(int id)
        {

            return Ok();

        }


        [HttpGet("unauthorized")]
        public async Task<IActionResult> GetUnAuthorizedError()
        {

            return Unauthorized();

        }






    }
}
