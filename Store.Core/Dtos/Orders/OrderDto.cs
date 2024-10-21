using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Dtos.Orders
{
    public class OrderDto
    {
        public string BasketId { get; set; }

        public int DeliveryMethodId { get; set; }

        public AddressDto ShippingAddress { get; set; }
    }
}
