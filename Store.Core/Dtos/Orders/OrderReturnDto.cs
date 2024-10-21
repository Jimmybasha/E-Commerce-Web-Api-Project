using Store.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Dtos.Orders
{
    public class OrderReturnDto
    {


        public int Id { get; set; } // To get the order by its Id

        public string BuyerEmail { get; set; }

        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;

        public string Status { get; set; }

        public AddressDto ShippingAddress { get; set; }

        public string DeliveryMethod { get; set; }
        public decimal DeliveryMethodCost { get; set; }

        public ICollection<OrderItemDto> Items { get; set; }

        public decimal Subtotal { get; set; }

        public decimal Total { get; set; }

        public string PaymentIntentId { get; set; } = string.Empty;

    }
}
