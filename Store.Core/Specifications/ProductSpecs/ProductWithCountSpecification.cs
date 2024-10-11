using Store.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Specifications.ProductSpecs
{
    public class ProductWithCountSpecification :BaseSpecifications<Product,int>
    {
        //To Just get the criteria none of any other Functions
        public ProductWithCountSpecification(ProductSpecParams productSpec) : base(
                    P =>
                          //if with null then it will return true it won't go to the second case and vice versa
                          (string.IsNullOrEmpty(productSpec.Search) || P.Name.ToLower().Contains(productSpec.Search))
                         &&
                          (!productSpec.BrandId.HasValue || productSpec.BrandId == P.BrandId) 
                          && (!productSpec.TypeId.HasValue || productSpec.TypeId == P.TypeId)

    ){
        }


    }
}
