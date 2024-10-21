using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Store.Core;
using Store.Core.Dtos.Basket;
using Store.Core.Entities;
using Store.Core.Entities.Order;
using Store.Core.Services.Contract;
using Store.Core.Specifications.OrderSpecs;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product = Store.Core.Entities.Product;

namespace Store.Service.Services.Payments
{
    public class PaymentService : IPaymentService
    {
        private readonly IBasketServices basketServices;
        private readonly IUnitOfWork unitOfWork;
        private readonly IConfiguration configuration;

        public PaymentService(
            IBasketServices basketServices,
            IUnitOfWork unitOfWork,
            IConfiguration configuration

            )
        {
            this.basketServices = basketServices;
            this.unitOfWork = unitOfWork;
            this.configuration = configuration;
        }

        public async Task<CustomerBasketDto> CreateOrUpdatePaymentIntentIdAsync(string basketId)
        {
            //SecretKey

            StripeConfiguration.ApiKey = configuration["Stripe:SecretKey"];

            //GetBasket

         var basket =  await basketServices.GetBasketAsync(basketId);

            if (basket is null) return null;

            var shippingPrice = 0m;

            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await unitOfWork.Repository<DeliveryMethod, int>().GetAsync(basket.DeliveryMethodId.Value);
                shippingPrice = deliveryMethod.Cost;
            }


            if (basket.Items.Count() > 0)
            {
                foreach (var item in basket.Items)
                {
                    
                var product = await unitOfWork.Repository<Product, int>().GetAsync(item.Id);

                    if (item.Price != product.Price)
                    {
                        item.Price = product.Price;
                    }
                }

            }

            var subtotal = basket.Items.Sum(I => I.Price * I.Quantity);


            var service = new PaymentIntentService();

            PaymentIntent paymentIntent;

            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                //Create
                var options = new PaymentIntentCreateOptions()
                {
                    //*100 to convert cents
                    Amount = (long)(subtotal * 100 + shippingPrice * 100),
                    PaymentMethodTypes = new List<string>(){"card"},
                    Currency = "usd"
                };
                paymentIntent = await service.CreateAsync(options);
                basket.ClientSecret = paymentIntent.ClientSecret;
                basket.PaymentIntentId = paymentIntent.Id;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    //*100 to convert cents
                    Amount = (long)(subtotal * 100 + shippingPrice * 100),
                };


                paymentIntent = await service.UpdateAsync(basket.PaymentIntentId,options);
                basket.ClientSecret = paymentIntent.ClientSecret; 
                basket.PaymentIntentId = paymentIntent.Id;
            }

            basket = await basketServices.UpdateBasketAsync(basket);

            if (basket is null) return null;

            return basket;

        }

        public async Task<Order> UpdatePaymentStatus(string paymentIntentId, bool flag)
        {
            var spec = new OrderSpecificationsWithPaymentIntentId(paymentIntentId);
            var order = await unitOfWork.Repository<Order, int>().GetWithSpecAsync(spec);

            if (order == null)
            {
                throw new Exception($"No order found with paymentIntentId: {paymentIntentId}");
            }

            if (flag)
            {
                order.Status = OrderStatus.PaymentReceived;
            }
            else
            {
                order.Status = OrderStatus.PaymentFailed;

            }
            unitOfWork.Repository<Order, int>().Update(order);

            await unitOfWork.CompleteAsync();

            return order;

        }
    }
}
