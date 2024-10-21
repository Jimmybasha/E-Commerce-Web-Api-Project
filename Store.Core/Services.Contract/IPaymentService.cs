using Store.Core.Dtos.Basket;
using Store.Core.Entities;
using Store.Core.Entities.Order;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Services.Contract
{
    public interface IPaymentService
    {

        Task<CustomerBasketDto> CreateOrUpdatePaymentIntentIdAsync(string basketId);

        Task<Order> UpdatePaymentStatus(string paymentIntentId,bool flag);


    }
}
