using Store.Core;
using Store.Core.Entities;
using Store.Core.Entities.Order;
using Store.Core.Repositories.Contract;
using Store.Core.Services.Contract;
using Store.Core.Specifications;
using Store.Core.Specifications.OrderSpecs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IBasketServices basketServices;
        private readonly IPaymentService paymentService;

        public OrderService(
            IUnitOfWork unitOfWork,
            IBasketServices basketServices,
            IPaymentService paymentService

            )
        {
            this.unitOfWork = unitOfWork;
            this.basketServices = basketServices;
            this.paymentService = paymentService;
        }

        public async Task<Order> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
        {

            //Get the basket items with the basketId
            var basketItems = await basketServices.GetBasketAsync(basketId);


            if (basketItems is null) return null;


            var orderItems = new List<OrderItem>();


            //If the basketItem isn't empty
            if (basketItems.Items.Count() > 0)
            {
                //Go for every item
                foreach (var item in basketItems.Items)
                {
                    //For every Item in the basket i'll save it into a variable
                    var product = await unitOfWork.Repository<Product, int>().GetAsync(item.Id);

                    //i'll use this variable to get the Specs of the item
                    var productOrderedItem = new ProductItemOrder(product.Id, product.Name, product.PictureUrl);

                    //put items that is needed to satisfy the items to be ordered , qty ,price , item itself
                    var orderItem = new OrderItem(productOrderedItem, product.Price, item.Quantity,item.ProductName);

                    //add them to the orderItemsList
                    orderItems.Add(orderItem);

                }
            }



            //Get Delivery method
            var deliveryMethod = await unitOfWork.Repository<DeliveryMethod, int>().GetAsync(deliveryMethodId);


            var subTotal = orderItems.Sum(I => I.Price * I.Quantity);

            //Send Payment


            if (basketItems.PaymentIntentId is not null)
            {
            var spec = new OrderSpecificationsWithPaymentIntentId(basketItems.PaymentIntentId);
            var ExistOrder = await unitOfWork.Repository<Order, int>().GetWithSpecAsync(spec);
            unitOfWork.Repository<Order, int>().Delete(ExistOrder);
            }
            
            var newPaymentIntentId = await paymentService.CreateOrUpdatePaymentIntentIdAsync(basketId);


            var order = new Order(buyerEmail,shippingAddress , deliveryMethod, orderItems, subTotal, newPaymentIntentId.PaymentIntentId, $"{buyerEmail.Split("@")[0]}");

            await unitOfWork.Repository<Order,int>().AddAsync(order);

            var result = await unitOfWork.CompleteAsync();
            
            if (result <= 0)  return null;

            return order;
        }

        public async Task<Order?> GetOrdersByIdForSpecificUser(string buyerEmail, int orderId)
        {

            var spec = new OrderSpecification(buyerEmail, orderId);

            var order = await unitOfWork.Repository<Order, int>().GetWithSpecAsync(spec);

            if (order is null) return null;

            return order;
        }

        public async Task<IEnumerable<Order>?> GetOrdersForSpecificUserAsync(string buyerEmail)
        {
             
            var spec = new OrderSpecification(buyerEmail);
            
            var orders = await unitOfWork.Repository<Order, int>().GetAllWithSpecAsync(spec);

            if (orders is null) return null;

            return orders;




        }
    }
}
