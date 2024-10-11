using Store.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Specifications.ProductSpecs
{

    public class ProductSpecification : BaseSpecifications<Product, int>
    {
        public ProductSpecification(ProductSpecParams productSpec) :base(
           
            P=>
            //if with null then it will return true it won't go to the second case and vice versa
            (string.IsNullOrEmpty(productSpec.Search) || P.Name.ToLower().Contains(productSpec.Search))
            &&
            (!productSpec.BrandId.HasValue || productSpec.BrandId == P.BrandId) 
            && (!productSpec.TypeId.HasValue || productSpec.TypeId == P.TypeId)

            )

        {
            

            if (!string.IsNullOrEmpty(productSpec.Sort))
            {
                //name , priceAsc,priceAsc
                switch (productSpec.Sort)
                {
                    case "PriceAsc":
                        AddOrderBy(P => P.Price);
                        break;
                    case "PriceDesc":
                        AddOrderByDescending(P => P.Price);
                        break;
                    default:
                        AddOrderBy(P => P.Name);
                        break;
                }
            }
            else
            {
                AddOrderBy(P => P.Name);
            }


            ApplyIncludes();

            ApplyPagination(productSpec.PageSize.Value * (productSpec.PageIndex.Value - 1), productSpec.PageSize.Value);
        }
        public ProductSpecification(int Id) :base (P=>P.Id == Id)
        { 
            ApplyIncludes();
        }

        private void ApplyIncludes()
        {

            Includes.Add(P => P.Brand);
            Includes.Add(P => P.Type);
        }



    }
}
