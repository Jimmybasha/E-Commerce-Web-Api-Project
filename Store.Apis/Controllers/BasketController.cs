using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Apis.Errors;
using Store.Core.Dtos.Basket;
using Store.Core.Entities;
using Store.Core.Repositories.Contract;

namespace Store.Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository basketRepository;
        private readonly IMapper mapper;

        public BasketController(IBasketRepository basketRepository,IMapper mapper)
        {
            this.basketRepository = basketRepository;
            this.mapper = mapper;
        }

        [HttpGet] //Get: /api/Basket?basketId
        public async Task<ActionResult<CustomerBasket>> GetBasket(string? basketId)
        {
            if (basketId is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest, "Invalid Id"));

            var basket = await basketRepository.GetBasketAsync(basketId);

            if(basket is null) new CustomerBasket() { Id = basketId };

            return Ok(basket);
        }


        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdateBasket(CustomerBasketDto model)
        {

            var basket = await basketRepository.UpdateBasketAsync(mapper.Map<CustomerBasket>(model));

            if (basket is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            return Ok(basket);

        }


        [HttpDelete]
        public async Task DeleteBasket(string basketId)
        {
             await basketRepository.DeleteBasketAsync(basketId);

        }




    }
}
