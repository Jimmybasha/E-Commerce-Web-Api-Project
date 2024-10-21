using Store.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Specifications.OrderSpecs
{
    public class OrderSpecification :BaseSpecifications<Order,int>
    {

        public OrderSpecification(string buyerEmail) : 
            base(O => O.BuyerEmail == buyerEmail)
        {
            ApplyIncludes();
        }



    public OrderSpecification(string buyerEmail,int orderId) :
            base(O => O.BuyerEmail == buyerEmail && O.Id == orderId)
        {
            ApplyIncludes();
        }

        private void ApplyIncludes()
        {

            Includes.Add(P => P.DeliveryMethod);
            Includes.Add(P => P.Items);
        }
    }

}
