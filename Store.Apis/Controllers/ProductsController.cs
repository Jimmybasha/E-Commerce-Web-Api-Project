using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Apis.Errors;
using Store.Core;
using Store.Core.Dtos.Products;
using Store.Core.Helper;
using Store.Core.Services.Contract;
using Store.Core.Specifications.ProductSpecs;
using Store.Service.Services.Products;

namespace Store.Apis.Controllers
{
   
    public class ProductsController : BaseApiController
    {
        private readonly IProductService productService;

        public ProductsController(IProductService productService)
        {
            this.productService = productService;
        }



        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int? id) // Endpoint
        {
            if (id is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));
            var result = await productService.getProductByIdAsync(id.Value);
            if (result is null) return NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound));
            return Ok(result);

        }


        [ProducesResponseType(typeof(PaginationResponse<ProductDto>), StatusCodes.Status200OK)]
        [HttpGet]
        //sort :name,priceAsc,priceDesc
        public async Task<ActionResult<PaginationResponse<ProductDto>>> GetAllProducts([FromQuery]ProductSpecParams productSpec) // Endpoint 
        { 

            var result = await productService.getAllProductsAsync(productSpec);
            return Ok(result);

        }



        [ProducesResponseType(typeof(PaginationResponse<TypeBrandDto>), StatusCodes.Status200OK)]
        [HttpGet("types")]
        public async Task<ActionResult<IEnumerable<TypeBrandDto>>> GetAllTypes() // Endpoint
        {

            var result = await productService.getAllTypesAsync();
            return Ok(result);

        }

        [ProducesResponseType(typeof(PaginationResponse<TypeBrandDto>), StatusCodes.Status200OK)]
        [HttpGet("brands")]
        public async Task<ActionResult<IEnumerable<TypeBrandDto>>> GetAllBrands() // Endpoint
        {

            var result = await productService.getAllBrandsAsync();
            return Ok(result);

        }
    



    }
}
