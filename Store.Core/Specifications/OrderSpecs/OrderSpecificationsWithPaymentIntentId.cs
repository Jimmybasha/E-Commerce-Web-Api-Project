using Store.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Specifications.OrderSpecs
{
    public class OrderSpecificationsWithPaymentIntentId : BaseSpecifications<Order, int>
    {

        public OrderSpecificationsWithPaymentIntentId(string paymentIntentId) :base(O=>O.PaymentIntentId==paymentIntentId)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);

        }

    }
}
