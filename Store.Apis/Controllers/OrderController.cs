using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Apis.Errors;
using Store.Core;
using Store.Core.Dtos.Orders;
using Store.Core.Entities.Order;
using Store.Core.Services.Contract;
using System.Security.Claims;

namespace Store.Apis.Controllers
{
    public class OrderController : BaseApiController
    {
        private readonly IOrderService orderService;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;

        public OrderController(
            IOrderService orderService,
            IMapper mapper,
            IUnitOfWork unitOfWork
            
            )
        {
            this.orderService = orderService;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderDto model)
         {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            if (userEmail is null) return Unauthorized(new ApiErrorResponse(StatusCodes.Status401Unauthorized));

            var shippingaddress = mapper.Map<Address>(model.ShippingAddress);

            var order = await orderService.CreateOrderAsync(userEmail, model.BasketId, model.DeliveryMethodId , shippingaddress);

            if (order is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));


             return Ok(mapper.Map<OrderReturnDto>(order));

        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetOrderForSpecificUser()
        {

            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            if (userEmail is null) return Unauthorized(new ApiErrorResponse(StatusCodes.Status401Unauthorized));


            var orders = await orderService.GetOrdersForSpecificUserAsync(userEmail);

            if (orders is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            return Ok(mapper.Map<IEnumerable<OrderReturnDto>>(orders));


        }
        [Authorize]
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {

            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            if (userEmail is null) return Unauthorized(new ApiErrorResponse(StatusCodes.Status401Unauthorized));


            var orders = await orderService.GetOrdersByIdForSpecificUser(userEmail, orderId);

            if (orders is null) return NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound));

            return Ok(mapper.Map<OrderReturnDto>(orders));


        }


        [HttpGet("DeliveryMethods")]
        public async Task<IActionResult> GetDeliveryMethods()
        {


            var deliveryMethod = await unitOfWork.Repository<DeliveryMethod, int>().GetAllAsync();

            if (deliveryMethod is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            return Ok(deliveryMethod);


        }




    }
}
