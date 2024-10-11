using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Core;
using Store.Core.Services.Contract;
using Store.Core.Specifications.ProductSpecs;
using Store.Service.Services.Products;

namespace Store.Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductsController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int? id) // Endpoint
        {
            if (id is null) return BadRequest("Invalid id");
            var result = await productService.getProductByIdAsync(id.Value);
            if (result is null) return NotFound($"The Product with id {id} not found at Db");
            return Ok(result);

        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts([FromQuery]ProductSpecParams productSpec) // Endpoint 
        { 

            var result = await productService.getAllProductsAsync(productSpec);
            return Ok(result);

        }

        [HttpGet("types")]
        public async Task<IActionResult> GetAllTypes() // Endpoint
        {

            var result = await productService.getAllTypesAsync();
            return Ok(result);

        }
        [HttpGet("brands")]
        public async Task<IActionResult> GetAllBrands() // Endpoint
        {

            var result = await productService.getAllBrandsAsync();
            return Ok(result);

        }
    



    }
}
