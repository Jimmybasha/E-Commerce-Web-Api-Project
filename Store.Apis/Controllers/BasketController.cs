using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Apis.Errors;
using Store.Core.Dtos.Basket;
using Store.Core.Entities;
using Store.Core.Repositories.Contract;
using Store.Core.Services.Contract;
using Store.Service.Services.Basket;

namespace Store.Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketServices basketServices;

        public BasketController(IBasketServices basketServices)
        {
            this.basketServices = basketServices;
        }

        [HttpGet] //Get: /api/Basket?basketId
        public async Task<IActionResult> GetBasket(string? basketId)
        {

            if (basketId is null)  return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest, "Invalid Id"));

            var basket = await basketServices.GetBasketAsync(basketId);

            if(basket is null) NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound));

            return Ok(basket);

        }


        [HttpPost]
        public async Task<IActionResult> CreateOrUpdateBasket(CustomerBasketDto model)
        {

            var basket = await basketServices.UpdateBasketAsync(model);

            if (basket is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            return Ok(basket);

        }


        [HttpDelete]
        public async Task<IActionResult> DeleteBasket(string basketId)
        {
            if (basketId is null) return BadRequest(new ApiErrorResponse(400));

            var flag = await basketServices.DeleteBasketAsync(basketId);

            if (flag is false) return BadRequest(new ApiErrorResponse(400));
            
            return NoContent();
        }




    }
}
